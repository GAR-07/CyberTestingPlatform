import { Inject, Injectable } from '@angular/core';
import { RESOURSE_API_URL } from '../app-injection-tokens';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CourseData } from '../interfaces/courseData.model';
import { LectureData } from '../interfaces/lectureData.model';
import { TestData } from '../interfaces/testData.model';

@Injectable({
  providedIn: 'root'
})

export class StorageService {

  constructor(
    @Inject(RESOURSE_API_URL) private resourseApiUrl: string,
    private http: HttpClient
    ) { }

    uploadFiles(formData : FormData) {
      return this.http.post<any>(this.resourseApiUrl + '/Storage/UploadFiles', formData, { 
        reportProgress: true, 
        observe: 'events' 
      });
    }

    // Далее идут методы для курсов

    getCourses(sampleSize: number, page: number) {
      const params = new HttpParams()
        .set('Content-Type', 'application/json')
        .set('sampleSize', sampleSize.toString())
        .set('page', page.toString());
      return this.http.get<any>(this.resourseApiUrl + '/Course/GetCourses', { params });
    }

    getCourse(id: string) {
      return this.http.get<any>(this.resourseApiUrl + '/Course/GetCourse/' + id);
    }

    createCourse(course: CourseData) {
      return this.http.post<any>(this.resourseApiUrl + '/Course/CreateCourse', course);
    }

    updateCourse(id: string, course: CourseData) {
      return this.http.put<any>(this.resourseApiUrl + '/Course/UpdateCourse/' + id, course);
    }

    deleteCourse(id: string) {
      return this.http.delete<any>(this.resourseApiUrl + '/Course/DeleteCourse/' + id);
    }

    // Далее идут методы для лекций

    getLectures(sampleSize: number, page: number) {
      const params = new HttpParams()
        .set('Content-Type', 'application/json')
        .set('sampleSize', sampleSize.toString())
        .set('page', page.toString());
      return this.http.get<any>(this.resourseApiUrl + '/Lecture/GetLectures', { params });
    }

    getLecture(id: string) {
      return this.http.get<any>(this.resourseApiUrl + '/Lecture/GetLecture/' + id);
    }

    createLecture(lecture: LectureData) {
      return this.http.post<any>(this.resourseApiUrl + '/Lecture/CreateLecture', lecture);
    }

    updateLecture(id: string, lecture: LectureData) {
      return this.http.put<any>(this.resourseApiUrl + '/Lecture/UpdateLecture/' + id, lecture);
    }

    deleteLecture(id: string) {
      return this.http.delete<any>(this.resourseApiUrl + '/Lecture/DeleteLecture/' + id);
    }

    // Далее идут методы для тестов

    getTests(sampleSize: number, page: number) {
      const params = new HttpParams()
        .set('Content-Type', 'application/json')
        .set('sampleSize', sampleSize.toString())
        .set('page', page.toString());
      return this.http.get<any>(this.resourseApiUrl + '/Test/GetTests', { params });
    }

    getTest(id: string) {
      return this.http.get<any>(this.resourseApiUrl + '/Test/GetTest/' + id);
    }

    createTest(test: TestData) {
      return this.http.post<any>(this.resourseApiUrl + '/Test/CreateTest', test);
    }

    updateTest(id: string, test: TestData) {
      return this.http.put<any>(this.resourseApiUrl + '/Test/UpdateTest/' + id, test);
    }

    deleteTest(id: string) {
      return this.http.delete<any>(this.resourseApiUrl + '/Test/DeleteTest/' + id);
    }
}
