import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface ICustomer {
  CustomerName: string;
  Country: string;
}

@Component({
  selector: 'app-root',
  templateUrl: `app.component.html`,
  styles: []
})
export class AppComponent {
  private readonly baseUrl = "http://localhost:8082/odata/";

  public customers: ICustomer[];

  constructor(private http: HttpClient) {
  }

  public async loadData() {
    const data = (await this.http.get<{value: ICustomer[]}>(`${this.baseUrl}customers`)
      .toPromise()).value;
    this.customers = data;
  }
}
