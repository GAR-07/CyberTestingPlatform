export class CourseData {
  constructor(
    public id: string,
    public theme: string,
    public title: string,
    public description: string,
    public content: string,
    public price: number,
    public imagePath: string,
    public creatorID: string,
    public creationDate: string,
    public lastUpdationDate: string,
  ) {  }
}