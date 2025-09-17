import { Component, computed, inject, input } from '@angular/core';
import { Member } from '../../../types/member';
import { RouterLink } from '@angular/router';
import { AgePipe } from '../../../core/pipes/age-pipe';
import { LikesService } from '../../../core/services/likes-service';
import { PresenceService } from '../../../core/services/presence-service';

@Component({
  selector: 'app-member-card',
  imports: [RouterLink, AgePipe],
  templateUrl: './member-card.html',
  styleUrl: './member-card.css'
})
export class MemberCard {
  member = input.required<Member>();
  private likeService = inject(LikesService);
  protected hasLiked = computed(() => this.likeService.likesIds().includes(this.member().id));
  private presenceService = inject(PresenceService);
  protected isOnline = computed(() => this.presenceService.onlineUsers().includes(this.member().id));

  toggleLike(event: Event) {
    event.stopPropagation();
    this.likeService.toggleLike(this.member().id);
  }
}
