"""
Post the conversation to GitHub issue #15 as individual comments.
Run with: py post_comments.py
"""
import json, urllib.request, urllib.error, time

TOKEN = "<redacted>"  # GitHub personal access token with repo permissions
ISSUE_URL = "https://api.github.com/repos/acnicholls/Cypher/issues/15/comments"

COMMENTS = [

# ── 1. User opening message ───────────────────────────────────────────────────
"""---
### 💬 **User** — Feb 28, 2026

> https://github.com/acnicholls/Cypher
>
> `116 61 111 80 77 104 91 81 78 104 89 88 93 56 66 55 35 79 108 101 81 82 42 127 124 40 48 47 90 87 45 88 51 114 116 118 112 63 58 64`
>
> can you solve this code, given the understanding that each number might be a word?
>
> I have attempted to write a word solver in the current repo, but I cannot possibly search the amount of responses for meaningful intent due to the massive number of possibilities.""",


# ── 2. GitHub Copilot (Claude) full first response ────────────────────────────
"""---
### 🤖 **GitHub Copilot (Claude Sonnet 4.6)** — Feb 28, 2026

**Thought process:**

Let me fetch the repository and examine the cipher code to understand the encoding scheme. I ran a semantic search across the workspace and fetched the GitHub README simultaneously.

After reading the code I found the key in `dev/cypher_info/cypherInfo/enums.cs`:

```csharp
public enum letterValue {
    a = 1, b = 2, c = 3, ... z = 26
}
```

And in `frmMain.cs` the encrypt button sums letter values per word:
```csharp
int value = dict.FindWordValue(s);   // sum of letter values
this.txtOutputCode.Text += value.ToString() + " ";
```

So the scheme is: **each number = sum of letter values (a=1…z=26) for one word**. The message is 40 numbers → 40 words.

---

**Solver approach:**

I wrote three Python solvers against the project's own `enable1.txt` dictionary (172,820 words, 303 distinct value buckets):

1. **`solver.py`** — hand-crafted common-word heuristic + beam search (beam=500)
2. **`solver2.py`** — NLTK Brown corpus unigram frequency beam search (beam=2000)
3. **`solver3.py`** — NLTK Brown corpus **bigram** language model beam search (beam=3000) ← best

The simple scorer picked obscure short words. I rewrote with NLTK word frequency scoring for a proper language-model approach, then enhanced with bigram context scoring to improve sequential coherence.

---

**Results — High-confidence anchors** (stable across all three methods):

| Pos | Value | Word | Notes |
|-----|-------|------|-------|
| 1 | 116 | **without** | Top unigram frequency (583) |
| 2 | 61 | **you** | Top unigram by far (3286) |
| 9 | 78 | **little** | Top unigram (831) |
| 17 | 35 | **to** | Near-certain — 26,158 corpus occurrences; only 203 words match value 35 |
| 18 | 79 | **your** | Top unigram (923) |
| 23 | 42 | **new** | Top unigram (1635) |
| 26 | 40 | **any** | Top unigram (1344) |
| 28 | 47 | **time** | Top unigram (1598) |
| 30 | 87 | **nothing** | Top unigram (411) |
| 33 | 51 | **which** | Top unigram (3561) |
| 34 | 114 | **history** | Only common word at value 114 |
| 38 | 63 | **know** | Top unigram (683) |
| 39 | 58 | **they** | Top unigram (3620) |

---

**Best candidate sentence (bigram beam search #1):**
> without you suppose subject matter experience future course little experience religion program provided this type could **to** your business system another young new industrial countries any long time praying nothing both parties which history without sometimes inexplicable know they passed

**Greedy unigram best:**
> without you usually almost group experience probably another little experience study program outside this only could **to** your business system another young new industrial countries any its time morning nothing much program which history without sometimes slightly know they never

---

**Limitation:** Without knowing the specific word list used to *encode* the message, or any context about it, the correct sentence cannot be definitively identified. The beam search finds the statistically most plausible English sentence — not necessarily the original. This is the fundamental property that makes the cipher strong.

*Solvers written: `solver.py`, `solver2.py`, `solver3.py`*""",


# ── 3. User — permutations question ──────────────────────────────────────────
"""---
### 💬 **User** — Feb 28, 2026

can you print the name of the number of permutations for the matching words. the best I get is a exponential number from the math, I don't know the actual number of zeros....""",


# ── 4. GitHub Copilot — permutations answer ───────────────────────────────────
"""---
### 🤖 **GitHub Copilot (Claude Sonnet 4.6)** — Feb 28, 2026

**Thought process:**

Python uses arbitrary-precision integers natively, so multiplying all the per-position word counts together gives the exact integer — no floating point rounding, no scientific notation. I ran:

```python
total = 1
for v in MESSAGE:
    total *= len(val_to_words[v])
print(total)
print(f'That is a {len(str(total))}-digit number')
```

---

**Result — exact permutation count:**

```
31503342665152611694943866881611376992736636050112139588586739175014821867323947
06983643074970921125296472064000000000000000
```

That is a **124-digit number** (~3.15 × 10¹²³).

Per-position breakdown:
```
[ 1] value=116  1656 words      [21] value= 81  1696 words
[ 2] value= 61  1089 words      [22] value= 82  1750 words
[ 3] value=111  1681 words      [23] value= 42   404 words
[ 4] value= 80  1715 words      [24] value=127  1345 words
[ 5] value= 77  1598 words      [25] value=124  1375 words
[ 6] value=104  1866 words      [26] value= 40   358 words
[ 7] value= 91  1813 words      [27] value= 48   579 words
[ 8] value= 81  1696 words      [28] value= 47   533 words
[ 9] value= 78  1595 words      [29] value= 90  1822 words
[10] value=104  1866 words      [30] value= 87  1783 words
[11] value= 89  1792 words      [31] value= 45   418 words
[12] value= 88  1876 words      [32] value= 88  1876 words
[13] value= 93  1965 words      [33] value= 51   647 words
[14] value= 56   895 words      [34] value=114  1713 words
[15] value= 66  1234 words      [35] value=116  1656 words
[16] value= 55   831 words      [36] value=118  1541 words
[17] value= 35   203 words  ← tightest constraint (bottleneck)
[18] value= 79  1715 words      [37] value=112  1650 words
[19] value=108  1676 words      [38] value= 63  1121 words
[20] value=101  1865 words      [39] value= 58   927 words
                                [40] value= 64  1112 words
```

Position 17 (value=35, only 203 matching words) is the bottleneck — and `"to"` is near-certain there (26,158 corpus occurrences vs the next best option).

This aligns with your earlier Reddit comment of `5.04E+124` — the difference is the dictionary size used.""",


# ── 5. User — exhaustive search question ─────────────────────────────────────
"""---
### 💬 **User** — Feb 28, 2026

how long would it take you to solve it with exhaustive search?""",


# ── 6. GitHub Copilot — exhaustive search answer ─────────────────────────────
"""---
### 🤖 **GitHub Copilot (Claude Sonnet 4.6)** — Feb 28, 2026

**Thought process:**

I benchmarked the fastest possible check — just joining 40 pre-selected words into a string, no NLP scoring at all — to get an upper bound on throughput. Then divided the exact permutation count by that rate.

```python
N = 10_000_000
t0 = time.perf_counter()
for _ in range(N):
    s = ' '.join(sample_words)   # cheapest possible 'check'
elapsed = time.perf_counter() - t0
rate = N / elapsed
# → 1,683,799 permutations/second
```

Then used `math.log10` to express the time in human-meaningful scales without overflow.

---

**Result:**

Benchmark: **~1,683,799 permutation checks/second** (string-join only, no scoring — real scoring would be ~100–1000× slower).

| Scenario | Time |
|---|---|
| This machine alone | **10¹⁰⁹·⁸ years** |
| Ages of the universe that represents | **10⁹⁹·⁶ universe lifetimes** |
| Every atom in the observable universe (~10⁸⁰) acting as a computer at the same speed | **still 10³⁷ seconds** (10¹⁹·⁶ universe lifetimes) |

For context: the age of the universe is ~13.8 billion years (~10¹⁰ years). The exhaustive search would take **10⁹⁹ times longer than the universe has existed**, even parallelised across every atom that exists.

**Conclusion:** Exhaustive search is physically impossible. The cipher's strength comes entirely from the combinatorial explosion of word-value collisions, not a cryptographic key. The only viable approach is the beam search — exploiting language statistics to prune the search space from 10¹²³ down to a handful of plausible candidates.""",

]

def post_comment(body):
    data = json.dumps({"body": body}).encode("utf-8")
    req = urllib.request.Request(
        ISSUE_URL,
        data=data,
        headers={
            "Authorization": f"token {TOKEN}",
            "Content-Type": "application/json",
            "User-Agent": "cypher-solver",
        },
        method="POST",
    )
    with urllib.request.urlopen(req) as resp:
        result = json.loads(resp.read())
        return result["html_url"]

for i, comment in enumerate(COMMENTS, 1):
    print(f"Posting comment {i}/{len(COMMENTS)} ...", end=" ", flush=True)
    url = post_comment(comment)
    print(f"OK → {url}")
    time.sleep(1)   # be polite to the API

print("\nAll comments posted!")
