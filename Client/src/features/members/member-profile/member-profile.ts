import { Component, HostListener, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MemberService } from '../../../core/services/member-service';
import { EditableMember } from '../../../types/editableMember';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastService } from '../../../core/services/toast-service';
import { Member } from '../../../types/member';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-member-profile',
  imports: [DatePipe, FormsModule],
  templateUrl: './member-profile.html',
  styleUrl: './member-profile.css'
})
export class MemberProfile implements OnInit, OnDestroy {
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event: BeforeUnloadEvent) {
    if (this.editForm?.dirty) $event.preventDefault();
  }
  protected memberService = inject(MemberService);
  protected editableMember: EditableMember = {
    username: '',
    description: '',
    city: '',
    country: ''
  };
  private toast = inject(ToastService);
  private accountService = inject(AccountService);

  ngOnInit(): void {
    this.editableMember = {
      username: this.memberService.member()?.username || '',
      description: this.memberService.member()?.description || '',
      city: this.memberService.member()?.city || '',
      country: this.memberService.member()?.country || ''
    };
  }

  updateProfile() {
    if (!this.memberService.member()) return;

    const updatedMember = { ...this.memberService.member(), ...this.editableMember };
    this.memberService.updateMember(this.editableMember).subscribe({
      next: () => {
        const curretUser = this.accountService.currentUser();
        if (curretUser && updatedMember.username !== curretUser?.username) {
          curretUser.username = updatedMember.username;
          this.accountService.currentUser.set(curretUser);
          localStorage.setItem('user', JSON.stringify(curretUser));
        }

        this.toast.success('Profile updated successfully');
        this.memberService.editMode.set(false);
        this.memberService.member.set(updatedMember as Member);
        this.editForm?.reset(updatedMember);
      }
    });

  }

  ngOnDestroy(): void {
    if (this.memberService.editMode()) {
      this.memberService.editMode.set(false);
    }
  }
}
