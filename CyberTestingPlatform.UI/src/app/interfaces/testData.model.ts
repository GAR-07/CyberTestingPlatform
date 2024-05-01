export class TestData {
  constructor(
      public id: string,
      public theme: string,
      public title: string,
      public questions: string,
      public answerOptions: string,
      public correctAnswers: string,
      public position: number,
      public creatorId: string,
      public creationDate: string,
      public lastUpdationDate: string,
      public courseId: string
  ) {  }
}