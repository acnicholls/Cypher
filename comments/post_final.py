"""Post the final Q&A pair to GitHub issue #15."""
import json, urllib.request, time

TOKEN = "<redacted>"  # GitHub personal access token with repo permissions
ISSUE_URL = "https://api.github.com/repos/acnicholls/Cypher/issues/15/comments"

COMMENTS = [

# ── 7. User ───────────────────────────────────────────────────────────────────
"""---
### 💬 **User** — Feb 28, 2026

one last question, and I would like this question, and your answer also posted to the issue.

how long would it have taken my program to WRITE all the permutations to a medium observable by a human, and then by you, or all the atoms of the universe?""",


# ── 8. GitHub Copilot ─────────────────────────────────────────────────────────
"""---
### 🤖 **GitHub Copilot (Claude Sonnet 4.6)** — Feb 28, 2026

**Thought process:**

Before even asking "how long to write?", there is a storage problem. Each sentence is ~280 bytes (40 words × avg 7 characters). Multiplying by the total permutation count:

- **Total data = 10^126.9 bytes**
- Observable universe atoms (at 1 byte/atom) = **10^80 bytes**
- The data is **10^47 times larger than the entire observable universe**

There is literally nowhere in existence to store it all. But if we ignore that and just ask about the writing rate:

---

**Time to write all 3.15 × 10¹²³ permutations:**

| Medium / Writer | Rate (sentences/sec) | Time (log₁₀ years) | Universe lifetimes |
|---|---|---|---|
| Human writing by hand (30 wpm, 40 words/sentence) | 0.75 | **10¹¹⁷·¹ years** | 10¹⁰⁷·⁰ universes |
| Human reading each one (250 wpm) | 0.10 | **10¹¹⁸·⁰ years** | 10¹⁰⁷·⁸ universes |
| Laser printer (20 pages/min, ~50 sentences/page) | 16.7 | **10¹¹⁵·⁸ years** | 10¹⁰⁵·⁶ universes |
| **Your program writing to SQL DB (~5,000 inserts/sec)** | 5,000 | **10¹¹³·³ years** | 10¹⁰³·² universes |
| SSD at full write speed (500 MB/s) | 1,785,714 | **10¹¹⁰·⁷ years** | 10¹⁰⁰·⁶ universes |
| Me reading tokens (~1,000 tokens/sec, 50 tokens/sentence) | 20 | **10¹¹⁵·⁷ years** | 10¹⁰⁵·⁶ universes |
| Every atom in universe as a computer at SSD speed (parallel) | 1.79 × 10⁸⁶ | **10³⁰·⁷ years** | **10²⁰·⁶ universes** |

---

**Key observations:**

1. **Your SQL program** would take 10¹⁰³ times the age of the universe — roughly the same order of magnitude as exhaustive *checking*, because the bottleneck is the count of permutations, not the I/O speed.

2. **Me reading every output** is about as slow as a human — token throughput is not that different from human reading speed at the scale of a 40-word sentence.

3. **The data physically cannot exist.** Even if writing were instantaneous, there is no medium in the universe large enough to hold 10¹²⁷ bytes. The observable universe contains ~10⁸⁰ atoms; using every atom as a single bit gives you only 10⁷⁶ bytes of storage — still 10⁵¹ times too small.

4. **Even with every atom as a parallel computer at SSD speed**, you still need 10²⁰·⁶ universe lifetimes — roughly 400 quintillion times the current age of the universe.

**This is the correct intuition for why the cipher works.** It isn't protected by a key or algorithm — it's protected by the sheer scale of the search space, which exceeds the computational capacity of the physical universe by many orders of magnitude.""",

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

for i, c in enumerate(COMMENTS, 7):
    print(f"Posting comment {i}/8 ...", end=" ", flush=True)
    print(f"OK -> {post(c)}")
    time.sleep(1)

print("Done!")
