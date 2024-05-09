import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { TestResultData } from 'src/app/interfaces/testResultData.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-test-result-block',
  templateUrl: './test-result-block.component.html',
  styleUrls: ['./test-result-block.component.scss']
})
export class TestResultBlockComponent {
  
  @Input() mods: string[] = ['full view'];
  @Input() test!: TestData;
  @Input() testResult!: TestResultData;

  mode: string = '';
  roles: string[] = [];
  currentGuid: string | null = null;
  answers: string[] = [];
  results: string[] = [];
  correctPersents: number = 0;

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private notificationService: NotificationService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    var accountData = this.authService.accountData();
    this.roles = accountData ? accountData.role : [];
    this.changeMode(this.mode);
    this.getComponentData();

    this.route.paramMap.subscribe(params => {
      const newGuid = params.get('guid');

      if (newGuid !== this.currentGuid) {
        this.currentGuid = newGuid;

        if (newGuid !== null) {
          this.getComponentData();
          console.log('update test!');
        }
      }
    });
  }

  changeMode(mode: string) {
    this.mode = this.mods.includes(mode) ? mode : this.mods[0];
  }

  goBack() {
    window.history.back();
  }

  async getComponentData() {
    if (!this.testResult) {
      this.currentGuid = this.route.snapshot.paramMap.get('guid');
      if (this.currentGuid !== null) {
        await this.getTestResult(this.currentGuid);
      }
    }
    if (!this.test && this.testResult) {
      await this.getTest(this.testResult.testId);
    }

    this.calculateAccuracyPercentage();
  }

  getTestResult(id: string) {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getTestResult(id).subscribe({
        next: (response: TestResultData) => {
          if (response.results !== null) {
            this.testResult = response;
            this.answers = response.answers.split('\n');
            this.results = response.results.split('\n');
          }
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
  }

  getTest(id: string) {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getTest(id).subscribe({
        next: (response: TestData) => {
          this.test = response;
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
  }

  calculateAccuracyPercentage(): number {
    const correctAnswersCount = this.results.filter(result => result === 'Верно').length;
  
    const totalAnswersCount = this.answers.length;
    const accuracyPercentage = (correctAnswersCount / totalAnswersCount) * 100;
  
    return accuracyPercentage;
  }
}
