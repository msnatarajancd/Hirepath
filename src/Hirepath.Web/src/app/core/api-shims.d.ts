// The NSwag-generated client (src/app/core/api, git-ignored) references a
// `FileParameter` type for the multipart upload but — with our .NET OpenAPI form
// shape — does not emit its definition. Declare it globally so the generated
// client type-checks. The upload method itself is not used; resume uploads go
// through HttpClient directly (see ResumeStore and ADR 0006).
declare interface FileParameter {
  data: unknown;
  fileName: string;
}
