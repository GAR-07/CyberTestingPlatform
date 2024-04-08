export class LectureData {
  constructor(
      public id: string,
      public theme: string,
      public title: string,
      public content: string,
      public position: number,
      public creatorId: string,
      public creationDate: string,
      public lastUpdationDate: string,
      public courseId: string
  ) {  }
}