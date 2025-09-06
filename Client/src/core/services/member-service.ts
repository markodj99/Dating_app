import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../../types/member';
import { Photo } from '../../types/photo';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  public editMode = signal(false);

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'member/all');
  }

  getMember(id: string) {
    return this.http.get<Member>(this.baseUrl + 'member/' + id);
  }

  getMemberPhotos(id: string) {
    return this.http.get<Photo[]>(this.baseUrl + 'member/' + id + '/photos');
  }
}
