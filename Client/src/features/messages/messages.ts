import { Component, inject, OnInit, signal } from '@angular/core';
import { MessageService } from '../../core/services/message-service';
import { PaginatedResult } from '../../types/pagination';
import { Message } from '../../types/message';
import { Paginator } from "../../shared/paginator/paginator";
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { ConfirmDialogService } from '../../core/services/confirm-dialog-service';

@Component({
  selector: 'app-messages',
  imports: [Paginator, RouterLink, DatePipe],
  templateUrl: './messages.html',
  styleUrl: './messages.css'
})
export class Messages implements OnInit {
  private messageService = inject(MessageService);
  protected cointainer = 'Inbox'
  protected fetchedContainer = "Inbox";
  protected pageNumber = 1;
  protected pageSize = 10;
  protected paginatedMessages = signal<PaginatedResult<Message> | null>(null);
  private confirmDialog = inject(ConfirmDialogService);

  tabs = [
    { label: 'Inbox', value: 'Inbox' },
    { label: 'Outbox', value: 'Outbox' }
  ];

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messageService.getMessages(this.cointainer, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.paginatedMessages.set(response);
        this.fetchedContainer = this.cointainer;
      }
    });
  }

  async confirmDelete(event: Event, id: string) {
    event.stopPropagation();
    const ok = await this.confirmDialog.confirm('Are you sure you want to delete this message?');
    if (ok) this.deleteMessage(id);
  }

  deleteMessage(id: string) {
    this.messageService.deleteMessage(id).subscribe({
      next: () => {
        const current = this.paginatedMessages();
        if (current?.items) {
          this.paginatedMessages.update(prev => {
            if (!prev) return null;
            const newItems = prev.items?.filter(x => x.id !== id) || [];
            const newMetadata = prev.metaData ? {
              ...prev.metaData,
              totalCount: prev.metaData.totalCount - 1,
              totalPages: Math.max(1, Math.ceil((prev.metaData.totalCount - 1) / prev.metaData.pageSize)),
              currentPage: Math.min( prev.metaData.currentPage, 
                Math.max(1, Math.ceil((prev.metaData.totalCount - 1) / prev.metaData.pageSize)))
            } : prev.metaData;
            return {
              items: newItems,
              metaData: newMetadata
            };
          });
        }
      }
    });
  }

  get isInbox() {
    return this.fetchedContainer === 'Inbox';
  }

  setContainer(container: string) {
    this.cointainer = container;
    this, this.pageNumber = 1;
    this.loadMessages();
  }

  onPageChange(event: { pageNumber: number, pageSize: number }) {
    this.pageNumber = event.pageNumber;
    this.pageSize = event.pageSize;
    this.loadMessages();
  }
}
