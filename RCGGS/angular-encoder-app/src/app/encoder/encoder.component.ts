import { Component, OnInit, OnDestroy, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-encoder',
  templateUrl: './encoder.component.html',
  styleUrls: ['./encoder.component.scss']
})
export class EncoderComponent implements OnInit, OnDestroy {
  inputText: string = '';
  encodedText: string = '';
  isEncoding: boolean = false;
  requestId: string | null = null;
  private hubConnection!: signalR.HubConnection;

  constructor(private http: HttpClient, private zone: NgZone) {}

  ngOnInit(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.encodingHubUrl)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('Connection started'))
      .catch(err => console.error('Error while starting connection: ' + err));

    this.hubConnection.on('ReceiveCharacter', (character: string, requestId: string) => {
      this.zone.run(() => { // This ensures change detection is triggered
        if (this.requestId === requestId) {
          this.encodedText += character;
        }
      });
    });
  }

  ngOnDestroy(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  async convertText(event: Event): Promise<void> {
    event.preventDefault();
    if (this.isEncoding) {
      return;
    }

    this.isEncoding = true;
    this.encodedText = '';
    try {
      const response = await firstValueFrom(
        this.http.post(`${environment.encodingHubUrl}/start`, { input: this.inputText }, { responseType: 'text' as 'json' })
      );
      this.requestId = response as string || null;
    } catch (error) {
      console.error(error);
      this.resetEncodingState();
    }
  }

  async cancelEncoding() {
    if (!this.requestId) return;
  
    this.isEncoding = false;
  
    try {
      await firstValueFrom(
        this.http.post(`${environment.encodingHubUrl}/cancel/${this.requestId}`, {})
      );
    } catch (error) {
      console.error(error);
    }

    this.resetEncodingState();
  }
  
  private resetEncodingState() {
    this.isEncoding = false;
    this.requestId = null;
  }
}
