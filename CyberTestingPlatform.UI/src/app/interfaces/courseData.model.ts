export class CourseData {
  constructor(
    public id: string,
    public name: string,
    public description: string,
    public price: number,
    public imagePath: string,
    public creatorId: string,
    public creationDate: string,
    public lastUpdationDate: string,
  ) {  }
}