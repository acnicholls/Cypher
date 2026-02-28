"""
Cypher Solver v3 – Bigram language model beam search
"""

import os, math, collections
import nltk
from nltk.corpus import brown
from nltk import bigrams as nltk_bigrams

MESSAGE = [116, 61, 111, 80, 77, 104, 91, 81, 78, 104,
           89, 88, 93, 56, 66, 55, 35, 79, 108, 101,
           81, 82, 42, 127, 124, 40, 48, 47, 90, 87,
           45, 88, 51, 114, 116, 118, 112, 63, 58, 64]

DICT_PATH = r"a:\source\githubRepos\Cypher\dev\POOL\enable1.txt"
BEAM_WIDTH = 3000
MAX_CANDIDATES = 200

def word_value(w):
    return sum(ord(c) - 96 for c in w.lower() if 'a' <= c <= 'z')

# ── Corpus frequencies ────────────────────────────────────────────────────────
print("Loading corpus …")
words_lower = [w.lower() for w in brown.words()]
unigram = collections.Counter(words_lower)
bigram  = collections.Counter(nltk_bigrams(words_lower))
total_uni = sum(unigram.values())
vocab = len(unigram)

ALPHA = 0.1   # bigram vs unigram mixture weight

def score_pair(prev_word, word):
    """Returns negative log-prob: lower = more probable."""
    uni_p = (unigram[word] + 1) / (total_uni + vocab)
    bi_count = bigram.get((prev_word, word), 0)
    bi_p = (bi_count + ALPHA) / (unigram.get(prev_word, 0) + ALPHA * vocab)
    # interpolation
    p = 0.3 * uni_p + 0.7 * bi_p
    return -math.log(p + 1e-12)

def score_first(word):
    p = (unigram[word] + 1) / (total_uni + vocab)
    return -math.log(p)

# ── Value map ─────────────────────────────────────────────────────────────────
print("Building value map …")
val_to_words = collections.defaultdict(list)
with open(DICT_PATH, encoding="utf-8", errors="ignore") as f:
    for line in f:
        w = line.strip().lower()
        if w and w.isalpha():
            val_to_words[word_value(w)].append(w)

# Sort candidates by unigram frequency
def uni_score(w): return -(unigram.get(w, 0))
for v in set(MESSAGE):
    val_to_words[v].sort(key=uni_score)

# ── Beam search ───────────────────────────────────────────────────────────────
print("Beam searching with bigrams …")

v0 = MESSAGE[0]
beams = [(score_first(w), w, [w]) for w in val_to_words[v0][:MAX_CANDIDATES]]
beams.sort()
beams = beams[:BEAM_WIDTH]

for step, v in enumerate(MESSAGE[1:], start=2):
    cands = val_to_words[v][:MAX_CANDIDATES] or [f"[{v}]"]
    new_beams = []
    for (sc, prev_w, words) in beams:
        for w in cands:
            ns = sc + score_pair(prev_w, w)
            new_beams.append((ns, w, words + [w]))
    new_beams.sort()
    beams = new_beams[:BEAM_WIDTH]
    if step % 8 == 0:
        top = ' '.join(beams[0][2][:8]) + '…'
        print(f"  step {step:2d}  top so far: {top}")

print("\n── Top 20 candidate sentences (bigram-scored) ──────────────────────────")
for rank, (sc, _, words) in enumerate(beams[:20], 1):
    print(f"  #{rank:2d}  {' '.join(words)}")
