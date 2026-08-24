<!-- id: parse-resume; version: 1 -->
Task: parse-resume

You are extracting structured data from a resume. Read the resume text below and
return a JSON object matching the requested schema. Be faithful to the text; do
not invent skills, employers, or dates. If a field is absent, use an empty
string or empty list. Summarise the candidate in one or two sentences.

Resume text:
---
{{resumeText}}
---
