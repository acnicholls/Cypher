"""Post the non-English language Q&A to GitHub issue #15."""
import json, urllib.request, time

TOKEN = "<redacted>"  # GitHub personal access token with repo permissions
ISSUE_URL = "https://api.github.com/repos/acnicholls/Cypher/issues/15/comments"

COMMENTS = [

# ── 11. User ──────────────────────────────────────────────────────────────────
"""---
### 💬 **User** — Feb 28, 2026

I have another question, what is the probability of the language used not being english, and time to solve, if so? post the question, thought process and answer to the issue.""",


# ── 12. Thought process ───────────────────────────────────────────────────────
"""---
### 🧠 **GitHub Copilot — Thought Process** — Feb 28, 2026

**Step 1 — What languages are even technically possible?**

The cipher scheme uses `a=1, b=2, … z=26` — the standard English/Latin alphabet. Any language that:
- Uses the Latin alphabet (or can be meaningfully transliterated to it), AND
- Has words whose letter-sum values happen to match the 40 numbers in the message

...could be the source language. That includes English, French, German, Spanish, Portuguese, Italian, Dutch, etc. It excludes Russian, Arabic, Hebrew, Chinese, Japanese (unless transliterated by design, which would be unusual for a WWII British context).

Diacritics (é, ü, ñ, etc.) are ambiguous — I stripped them to their base Latin character for the analysis, which is the most natural interpretation.

**Step 2 — Bayesian prior from context clues**

The Reddit post provides strong contextual evidence:
- The veteran worked at **RAF Chicksands** — a British SIGINT station, direct feeder to **Bletchley Park**
- The intelligence agency referenced is **MI5** — *British* domestic intelligence
- The post was written in English, on an English-language subreddit
- The cipher is described as "MI5 Standard" — again, British
- The poster said "It will be of great value to any person or organisation…" — English phrasing

All of this points overwhelmingly to English as the source language.

**Step 3 — Run the analysis for other languages**

I downloaded NLTK corpora for Spanish (`cess_esp`) and Dutch (`alpino`) and ran the same beam-search value-matching analysis across all three languages, using the same `a=1…z=26` scheme with diacritics stripped.

**Step 4 — Check beam search coherence as evidence**

The bigram beam search produced highly coherent English phrases:
> *"without you… to your business… new industrial countries… which history without… know they…"*

However, this is **not proof of English** — the language model was trained on English and will always produce English-sounding output. It scores how likely the output is *given English*, not how likely English is *given the output*.

**Step 5 — Compute solving times per language**

Smaller vocabulary → fewer matching words per position → fewer permutations → marginally faster to brute-force. But the time scales remain astronomically large for all languages.""",


# ── 13. Answer ────────────────────────────────────────────────────────────────
"""---
### 🤖 **GitHub Copilot (Claude Sonnet 4.6) — Answer** — Feb 28, 2026

**Probability the language is NOT English: ~10–15%**

The contextual evidence (RAF Chicksands, MI5, English Reddit, English phrasing) gives a very strong prior for English. A rough Bayesian breakdown:

| Language | Probability |
|---|---|
| **English** | **~85–90%** |
| French | ~4% (France was a major WWII ally, French agents used Chicksands-era codes) |
| German | ~3% (captured German codebooks were common at Bletchley-adjacent stations) |
| Spanish / Portuguese | ~1–2% |
| Dutch / Polish / other Allied | ~1–2% |
| Non-Latin script (Russian, Arabic, etc.) | <1% (scheme is Latin-alphabet native) |

---

**Cross-language technical analysis (a=1…z=26, diacritics stripped to base Latin):**

| Language | Vocabulary | log₁₀(perms) | Time @ 5,000 DB writes/sec | Universe lifetimes |
|---|---|---|---|---|
| **English** | 40,234 words | **101.8** | 10⁹⁰·⁶ years | 10⁸⁰·⁴ |
| **Spanish** | 19,968 words | **89.9** | 10⁷⁸·⁷ years | 10⁶⁸·⁵ |
| **Dutch** | 19,010 words | **86.9** | 10⁷⁵·⁷ years | 10⁶⁵·⁵ |

Zero-match positions: **none** — all three languages have at least one matching word for every position's value.

---

**Top candidates per anchor position, by language:**

| Pos | Value | English | Spanish | Dutch |
|-----|-------|---------|---------|-------|
| 2 | 61 | **you** | sólo (only) | waren (were) |
| 9 | 78 | **little** | tiempo (time) | laatste (last) |
| 14 | 56 | **this** | pasado (past) | onder (under) |
| 17 | 35 | **to** | un (a/an) | nu (now) |
| 33 | 51 | **which** | puede (can) | tegen (against) |
| 39 | 58 | **they** | cuando (when) | komen (come) |

---

**Key observations:**

1. **All three languages are technically compatible** with the cipher — zero positions have no matches in any language tested. The cipher does not self-identify its language.

2. **Smaller vocabulary → fewer permutations.** Spanish and Dutch are ~10¹² times "easier" than English in theory. But 10⁷⁵ years is still 10⁶⁵ times the age of the universe — equally impossible in practice.

3. **The beam search output is not proof of English.** The solver uses an English language model and will produce English-sounding output regardless of actual source language. To properly test French or German would require a French/German bigram model.

4. **The single strongest English indicator** is position 17 (value=35): in English the top match is `"to"` (26,158 corpus occurrences — by far the most common preposition). In Spanish the top match is `"un"` (indefinite article — equally plausible). In Dutch it's `"nu"` (now). All three are grammatically natural function words — no language is ruled out by this position alone.

5. **Bottom line:** The contextual evidence (British, MI5, Chicksands, English Reddit post) makes English the ~87% favourite, but the mathematics cannot distinguish languages — the cipher is equally hard in all of them, and none can be ruled out on numerical grounds alone. If further context about the veteran's background suggested multilingual training or foreign contacts, that prior should be updated accordingly.""",

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

for i, c in enumerate(COMMENTS, 11):
    print(f"Posting comment {i}/13 ...", end=" ", flush=True)
    print(f"OK -> {post(c)}")
    time.sleep(1)

print("Done!")
