"""
Cypher Solver v2 – NLTK word-frequency beam search
====================================================
Cipher rule: word_value = sum of letter values (a=1 … z=26)
Uses Brown corpus frequencies for beam scoring.
"""

import os, math, collections
import nltk
from nltk.corpus import brown

# ── Cipher constants ───────────────────────────────────────────────────────────
MESSAGE = [116, 61, 111, 80, 77, 104, 91, 81, 78, 104,
           89, 88, 93, 56, 66, 55, 35, 79, 108, 101,
           81, 82, 42, 127, 124, 40, 48, 47, 90, 87,
           45, 88, 51, 114, 116, 118, 112, 63, 58, 64]

DICT_PATH = r"a:\source\githubRepos\Cypher\dev\POOL\enable1.txt"
BEAM_WIDTH = 2000
MAX_CANDIDATES = 400   # per position

# ── Letter value ───────────────────────────────────────────────────────────────
def word_value(w):
    return sum(ord(c) - 96 for c in w.lower() if 'a' <= c <= 'z')

# ── Build value→words map from dictionary ────────────────────────────────────
print("Building value map …")
val_to_words = collections.defaultdict(list)
with open(DICT_PATH, encoding="utf-8", errors="ignore") as f:
    for line in f:
        w = line.strip().lower()
        if w and w.isalpha():
            val_to_words[word_value(w)].append(w)

# ── Build word frequency table from Brown corpus ─────────────────────────────
print("Building word frequencies from Brown corpus …")
freq = collections.Counter(w.lower() for w in brown.words())
total = sum(freq.values())

# log-probability with Laplace smoothing (add-1)
vocab_size = len(freq)
LOG_UNK = math.log(1 / (total + vocab_size))

def log_prob(w):
    return math.log((freq.get(w, 0) + 1) / (total + vocab_size))

# Rank every candidate word by log-prob, best (most negative log-no, highest
# absolute value) comes first – negate so lower = better for sorting.
def neg_log_prob(w):
    return -log_prob(w)

# ── Sort candidates per position by frequency ────────────────────────────────
print("Sorting candidates per position …")
sorted_candidates = {}
for v in set(MESSAGE):
    words = val_to_words.get(v, [])
    sorted_candidates[v] = sorted(words, key=neg_log_prob)[:MAX_CANDIDATES]

# ── Show top candidates per position ─────────────────────────────────────────
print("\nTop candidates per position (by frequency):")
for i, v in enumerate(MESSAGE):
    cands = sorted_candidates[v][:6]
    print(f"  [{i+1:2d}] v={v:3d}  {cands}")

# ── Beam search ───────────────────────────────────────────────────────────────
print("\nBeam searching …")

# beam: (cumulative_score, word_list)
# score = sum of negative-log-probs (lower = more probable sentence)
initial = sorted_candidates[MESSAGE[0]]
beams = [(-log_prob(w), [w]) for w in initial]
beams.sort()
beams = beams[:BEAM_WIDTH]

for step, v in enumerate(MESSAGE[1:], start=2):
    cands = sorted_candidates[v]
    if not cands:
        cands = [f"[{v}]"]
    new_beams = []
    for (sc, words) in beams:
        for w in cands:
            new_beams.append((sc - log_prob(w), words + [w]))
    new_beams.sort()
    beams = new_beams[:BEAM_WIDTH]
    if step % 8 == 0:
        print(f"  step {step}/{len(MESSAGE)}")

print("\n── Top 30 candidate sentences ──────────────────────────────────────────")
for rank, (sc, words) in enumerate(beams[:30], 1):
    print(f"  #{rank:2d} [{sc:7.1f}]  {' '.join(words)}")

# ── Also show the single best word (highest freq) per position ────────────────
print("\n── Greedy best-word per position ───────────────────────────────────────")
greedy = []
for i, v in enumerate(MESSAGE):
    best = sorted_candidates[v][0] if sorted_candidates[v] else f"[{v}]"
    freq_count = freq.get(best, 0)
    greedy.append(best)
    print(f"  [{i+1:2d}] v={v:3d}  best='{best}' (freq={freq_count})")
print("\nGreedy sentence:")
print(" ".join(greedy))
