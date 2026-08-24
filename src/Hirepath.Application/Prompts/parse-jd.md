<!-- id: parse-jd; version: 1 -->
Task: parse-jd

You are extracting structured data from a job description. Read the posting below
and return a JSON object matching the requested schema. Separate required from
preferred skills. Infer years required only if stated. List the ATS keywords a
resume would be screened against. Do not invent requirements the posting does not
state.

Job description text:
---
{{jdText}}
---
