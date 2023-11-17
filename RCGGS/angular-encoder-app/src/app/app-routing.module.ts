import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EncoderComponent } from './encoder/encoder.component'; 

const routes: Routes = [
  { path: 'encode', component: EncoderComponent },
  { path: '', redirectTo: '/encode', pathMatch: 'full' } // Redirect to encoder by default
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
