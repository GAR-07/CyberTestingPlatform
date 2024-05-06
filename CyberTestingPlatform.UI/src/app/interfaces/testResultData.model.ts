export class TestResultData {
  constructor(
      public id: string | null,
      public testId: string,
      public userId: string,
      public answers: string,
      public results: string | null,
      public creationDate: string | null
  ) {  }
}