"""Post the dictionary-size Q&A to GitHub issue #15."""
import json, urllib.request, time

TOKEN = "<redacted>"  # GitHub personal access token with repo permissions
ISSUE_URL = "https://api.github.com/repos/acnicholls/Cypher/issues/15/comments"

COMMENTS = [

# ── 9. User ───────────────────────────────────────────────────────────────────
"""---
### 💬 **User** — Feb 28, 2026

ok, what if I had 3 dictionaries of 100, 500, and 1000 words that were the only ones allowed to be used. also post this question and answer.""",


# ── 10. GitHub Copilot ────────────────────────────────────────────────────────
"""---
### 🤖 **GitHub Copilot (Claude Sonnet 4.6)** — Feb 28, 2026

**Thought process:**

I took the top-N most frequent English words from the Brown corpus (the same corpus used for the beam search scoring) and for each dictionary size, counted how many words in that set match each position's value. Then multiplied the per-position counts to get exact permutation totals, and divided by ~5,000 DB inserts/sec to get writing time.

The key new wrinkle: if a position's value has **zero matches** in the small dictionary, the message simply **cannot be represented** using that dictionary — it is undecipherable with those words.

---

**Results:**

| Dictionary size | Positions with matches | Zero-match positions | Total permutations | log₁₀(perms) | Time @ 5,000 DB inserts/sec | vs universe |
|---|---|---|---|---|---|---|
| **100 words** | 13 of 40 | **27 positions** | ~400 | 2.6 | **under 1 second** | — |
| **500 words** | 37 of 40 | 3 positions (24, 25, 37) | ~10²⁰ | 19.1 | **10⁷·⁹ years** | 10⁻²·³ universes |
| **1,000 words** | 39 of 40 | 1 position (37) | ~10³⁴ | 34.3 | **10²³·¹ years** | 10¹³·⁰ universes |

---

**100-word dictionary — full breakdown:**

| Pos | Value | Matches |
|-----|-------|---------|
| 1 | 116 | ❌ no match |
| 2 | 61 | `you` |
| 3 | 111 | ❌ no match |
| 4 | 80 | ❌ no match |
| 5 | 77 | ❌ no match |
| 6 | 104 | ❌ no match |
| 7 | 91 | ❌ no match |
| 8 | 81 | ❌ no match |
| 9 | 78 | ❌ no match |
| 10 | 104 | ❌ no match |
| 11 | 89 | ❌ no match |
| 12 | 88 | ❌ no match |
| 13 | 93 | ❌ no match |
| 14 | 56 | `will`, `this`, `out`, `there` |
| 15 | 66 | `other`, `only` |
| 16 | 55 | `could` |
| 17 | 35 | `to` |
| 18 | 79 | `your` |
| 19 | 108 | ❌ no match |
| 20 | 101 | ❌ no match |
| 21 | 81 | ❌ no match |
| 22 | 82 | ❌ no match |
| 23 | 42 | `new` |
| 24 | 127 | ❌ no match |
| 25 | 124 | ❌ no match |
| 26 | 40 | `any` |
| 27 | 48 | `its` |
| 28 | 47 | `time`, `then`, `also` |
| 29 | 90 | ❌ no match |
| 30 | 87 | ❌ no match |
| 31 | 45 | `much` |
| 32 | 88 | ❌ no match |
| 33 | 51 | `were`, `more`, `before`, `such`, `which` |
| 34 | 114 | ❌ no match |
| 35 | 116 | ❌ no match |
| 36 | 118 | ❌ no match |
| 37 | 112 | ❌ no match |
| 38 | 63 | ❌ no match |
| 39 | 58 | `into`, `they`, `two` |
| 40 | 64 | ❌ no match |

---

**Key observations:**

1. **100 words → the message cannot be decoded at all.** 27 of 40 positions have no matching word in the top-100. Where matches exist, only ~400 combinations exist — solvable in milliseconds — but the message is still undecipherable because most positions are blank.

2. **500 words → still effectively uncrackable.** Even with this small dictionary, 3 positions remain unresolvable and the remaining 37 produce ~10²⁰ combinations, which would take 10⁷·⁹ years at DB speed. Faster than the full dictionary, but still ~100 million times the age of the universe.

3. **1,000 words → one position still has no match (position 37, value=112), and the rest produce 10³⁴ combinations** — 10²³ years at DB speed.

4. **The implication for the cipher designer:** if they used a small private dictionary (e.g. 500 carefully chosen words), the message becomes *both* harder to decode (many zero-match positions with a standard dictionary) *and* the permutation space stays astronomically large for anyone who does have access to that same small dictionary. A 500-word shared codebook is genuinely more secure than the full English dictionary in this scheme.

5. **Position 37 (value=112)** is the hardest to cover — it has no match even in the top 1,000 most common English words. This means whatever word sits there is relatively uncommon, which is itself a clue.""",

]

def post(body):
    data = json.dumps({"body": body}).encode("utf-8")
    req = urllib.request.Request(
        ISSUE_URL, data=data,
        headers={"Authorization": f"token {TOKEN}", "Content-Type": "application/json", "User-Agent": "cypher-solver"},
        method="POST",
    )
    with urllib.request.urlopen(req) as resp:
        return json.loads(resp.read())["html_url"]

for i, c in enumerate(COMMENTS, 9):
    print(f"Posting comment {i}/10 ...", end=" ", flush=True)
    print(f"OK -> {post(c)}")
    time.sleep(1)

print("Done!")
