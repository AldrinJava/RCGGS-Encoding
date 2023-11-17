import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-encoder',
  templateUrl: './encoder.component.html',
  styleUrls: ['./encoder.component.scss']
})
export class EncoderComponent {
  inputText: string = '';
  encodedText: string = '';
  isEncoding: boolean = false;
  requestId: string | null = null;

  constructor(private http: HttpClient) {}

  async convertText() {
    if (this.isEncoding) {
      // Prevent starting a new process if one is already in progress
      return;
    }

    this.isEncoding = true;
    this.encodedText = '';
    try {
      const response = await firstValueFrom(
        this.http.post('https://localhost:7280/Encoding/start', { input: this.inputText }, { responseType: 'text' as 'json' })
      );
      this.requestId = response as string || null;
      this.getNextCharacter();
    } catch (error) {
      console.error(error);
      this.resetEncodingState();
    }
  }

  async getNextCharacter() {
    if (!this.requestId || !this.isEncoding) return;

    try {
      const character = await firstValueFrom(
        this.http.get(`https://localhost:7280/Encoding/get/${this.requestId}`, { responseType: 'text' })
      );
      if (character) {
        if (this.isEncoding){
          this.encodedText += character;
          this.getNextCharacter();
        }
      } else {
        this.isEncoding = false;
      }
    } catch (error) {
      console.error(error);
      this.resetEncodingState();
    }
  }

  async cancelEncoding() {
    if (!this.requestId) return;
  
    this.isEncoding = false; // Immediately stop any ongoing encoding process
  
    try {
      await firstValueFrom(
        this.http.post(`https://localhost:7280/Encoding/cancel/${this.requestId}`, {})
      );
    } catch (error) {
      console.error(error);
      // Handle error
    }
  }
  

  private resetEncodingState() {
    this.isEncoding = false;
    this.requestId = null;

  }
}
