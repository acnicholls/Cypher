"""
Cypher Solver
=============
Each number = sum of letter values (a=1, b=2, ..., z=26) for a word.
Uses beam search + word frequency to find the most plausible sentence.
"""

import os
import collections
import math
from itertools import islice

# ── Config ─────────────────────────────────────────────────────────────────────
MESSAGE = [116, 61, 111, 80, 77, 104, 91, 81, 78, 104,
           89, 88, 93, 56, 66, 55, 35, 79, 108, 101,
           81, 82, 42, 127, 124, 40, 48, 47, 90, 87,
           45, 88, 51, 114, 116, 118, 112, 63, 58, 64]

DICT_PATH = os.path.join(os.path.dirname(__file__), "dev", "POOL", "enable1.txt")
BEAM_WIDTH = 500          # number of beams to keep at each step
MAX_WORDS_PER_VALUE = 300 # cap candidates per position

# ── Letter value ───────────────────────────────────────────────────────────────
def word_value(word):
    total = 0
    for ch in word.lower():
        v = ord(ch) - 96   # a=1 … z=26
        if 1 <= v <= 26:
            total += v
    return total

# ── Load dictionary ────────────────────────────────────────────────────────────
print(f"Loading dictionary from: {DICT_PATH}")
if not os.path.exists(DICT_PATH):
    # fallback to the other copy
    DICT_PATH = os.path.join(os.path.dirname(__file__), "dev", "cypher_data", "cypherData", "enable1.txt")

val_to_words = collections.defaultdict(list)
word_count = 0
with open(DICT_PATH, encoding="utf-8", errors="ignore") as f:
    for line in f:
        w = line.strip().lower()
        if w and w.isalpha():
            val_to_words[word_value(w)].append(w)
            word_count += 1

print(f"Loaded {word_count} words covering {len(val_to_words)} distinct values.\n")

# ── Show candidates per position ───────────────────────────────────────────────
print("Candidates per position:")
for i, v in enumerate(MESSAGE):
    words = val_to_words.get(v, [])
    sample = words[:8]
    print(f"  [{i+1:2d}] value={v:3d}  ({len(words):4d} words)  e.g.: {sample}")

# ── Word frequency scoring (approximate: shorter common words score higher) ────
# We use a simple heuristic: prefer words that match common English patterns.
# Load a rough frequency ranking if available, else fall back to length-based.

# Common high-frequency English words get a bonus
COMMON = {
    "the","be","to","of","and","a","in","that","have","it","for","not","on","with",
    "he","as","you","do","at","this","but","his","by","from","they","we","say","her",
    "she","or","an","will","my","one","all","would","there","their","what","so","up",
    "out","if","about","who","get","which","go","me","when","make","can","like","time",
    "no","just","him","know","take","people","into","year","your","good","some","could",
    "them","see","other","than","then","now","look","only","come","its","over","think",
    "also","back","after","use","two","how","our","work","first","well","way","even",
    "new","want","because","any","these","give","day","most","us","great","between",
    "need","large","often","hand","high","place","hold","real","life","few","north",
    "open","seem","together","next","white","children","begin","got","walk","example",
    "ease","paper","group","always","music","those","both","mark","book","letter","until",
    "mile","river","car","feet","care","second","enough","plain","girl","usual","young",
    "ready","above","ever","red","list","though","feel","talk","bird","soon","body",
    "dog","family","direct","pose","leave","song","measure","door","product","black",
    "short","numeral","class","wind","question","happen","complete","ship","area","half",
    "rock","order","fire","south","problem","piece","told","knew","pass","since","top",
    "whole","king","space","heard","best","hour","better","true","during","hundred",
    "five","remember","step","early","hold","west","ground","interest","reach","fast",
    "verb","sing","listen","six","table","travel","less","morning","ten","simple",
    "several","vowel","toward","war","lay","against","pattern","slow","center","love",
    "person","money","serve","appear","road","map","rain","rule","govern","pull","cold",
    "notice","voice","unit","power","town","fine","drive","lead","cry","dark","machine",
    "note","wait","plan","figure","star","box","noun","field","rest","correct","oh",
    "able","pound","done","beauty","drive","stood","contain","front","teach","week",
    "final","gave","green","oh","quick","develop","ocean","warm","free","minute","strong",
    "special","mind","behind","clear","tail","produce","fact","street","inches","multiply",
    "nothing","course","stay","wheel","full","force","blue","object","decide","surface",
    "deep","moon","island","foot","system","busy","test","record","boat","common","gold",
    "possible","plane","age","dry","wonder","laugh","thousand","ago","ran","check","game",
    "shape","yes","hot","miss","brought","heat","snow","tire","bring","yes","distant",
    "fill","east","paint","language","among","grand","ball","yet","wave","drop","heart",
    "am","ring","present","heavy","dance","engine","position","arm","wide","sail","material",
    "fraction","forest","sit","race","window","store","summer","train","sleep","prove",
    "lone","leg","exercise","wall","catch","mount","wish","sky","board","joy","winter",
    "sat","written","wild","instrument","kept","glass","grass","cow","job","edge","sign",
    "visit","past","soft","fun","bright","gas","weather","month","million","bear","finish",
    "happy","hope","flower","clothe","strange","gone","jump","baby","eight","village",
    "meet","root","buy","raise","solve","metal","whether","push","seven","paragraph",
    "third","shall","held","hair","describe","cook","floor","either","result","burn",
    "hill","safe","cat","century","consider","type","law","bit","coast","copy","phrase",
    "silent","tall","sand","soil","roll","temperature","finger","industry","value","fight",
    "lie","beat","excite","natural","view","sense","capital","weight","time","am",
    "office","receive","row","mouth","exact","symbol","die","least","trouble","shout",
    "except","wrote","seed","tone","join","suggest","clean","break","lady","yard","rise",
    "bad","blow","oil","blood","touch","grew","cent","mix","team","wire","cost","lost",
    "brown","wear","garden","equal","sent","choose","fell","fit","flow","fair","bank",
    "collect","save","control","decimal","gentle","woman","captain","practice","separate",
    "difficult","doctor","please","protect","noon","whose","locate","ring","character",
    "insect","caught","period","indicate","radio","spoke","atom","human","history","effect",
    "electric","expect","crop","modern","element","hit","student","corner","party","supply",
    "bone","rail","imagine","provide","agree","thus","capital","chair","danger","fruit",
    "rich","thick","soldier","process","operate","guess","necessary","sharp","wing",
    "create","neighbor","wash","bat","rather","crowd","corn","compare","poem","string",
    "bell","depend","meat","rub","tube","famous","dollar","stream","fear","sight","thin",
    "triangle","planet","hurry","chief","colony","clock","mine","tie","enter","major",
    "fresh","search","send","yellow","gun","allow","print","dead","spot","desert","suit",
    "current","lift","rose","continue","block","chart","hat","sell","success","company",
    "subtract","event","particular","deal","swim","term","opposite","wife","shoe","shoulder",
    "spread","arrange","camp","invent","cotton","born","determine","quart","nine",
    "truck","noise","level","chance","gather","shop","stretch","throw","shine","property",
    "column","molecule","select","wrong","gray","repeat","require","broad","prepare",
    "salt","nose","plural","anger","claim","syllable","perhaps","pick","sudden","count",
    "square","reason","length","represent","art","subject","region","energy","hunt",
    "probable","bed","brother","egg","ride","cell","believe","fraction","forest",
}

def score_word(w):
    """Lower is better. Common short words get low scores."""
    base = -math.log(1 + len(COMMON)) if w in COMMON else 0
    # Prefer shorter words slightly (they tend to be more common)
    length_penalty = len(w) * 0.05
    return base + length_penalty

# ── Beam Search ────────────────────────────────────────────────────────────────
print("\nRunning beam search …")

# Beam: list of (score, [word, word, …])
initial_candidates = val_to_words.get(MESSAGE[0], [])
beams = []
for w in initial_candidates[:MAX_WORDS_PER_VALUE]:
    beams.append((score_word(w), [w]))

beams.sort(key=lambda x: x[0])
beams = beams[:BEAM_WIDTH]

for step, v in enumerate(MESSAGE[1:], start=2):
    candidates = val_to_words.get(v, [])
    if not candidates:
        candidates = [f"[{v}]"]  # placeholder if no match
    new_beams = []
    for (prev_score, prev_words) in beams:
        for w in candidates[:MAX_WORDS_PER_VALUE]:
            new_score = prev_score + score_word(w)
            new_beams.append((new_score, prev_words + [w]))
    new_beams.sort(key=lambda x: x[0])
    beams = new_beams[:BEAM_WIDTH]
    if step % 5 == 0:
        print(f"  step {step}/{len(MESSAGE)}  beam size={len(beams)}")

print("\n── Top 20 candidate sentences ──────────────────────────────────────────")
for rank, (sc, words) in enumerate(beams[:20], 1):
    print(f"  #{rank:2d}  {' '.join(words)}")

print("\n── Words in common set for each position ───────────────────────────────")
for i, v in enumerate(MESSAGE):
    common_matches = sorted(
        [w for w in val_to_words.get(v, []) if w in COMMON],
        key=lambda w: len(w)
    )
    all_matches = val_to_words.get(v, [])
    print(f"  [{i+1:2d}] v={v:3d}  common={common_matches}  (total {len(all_matches)})")
