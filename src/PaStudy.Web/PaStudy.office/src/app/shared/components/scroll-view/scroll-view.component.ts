import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { NgScrollbar, NgScrollbarModule } from 'ngx-scrollbar';

@Component({
  selector: 'app-scroll-view',
  standalone: true,
  imports: [NgScrollbarModule],
  templateUrl: './scroll-view.component.html',
  styleUrl: './scroll-view.component.scss',
})
export class ScrollViewComponent implements AfterViewInit {
  @ViewChild(NgScrollbar) scrollBarRef!: NgScrollbar;
  @Input() onScrollBottom: () => void = () => false;
  @Input() onScrollTop: () => void = () => false;

  public autoHeughtDisabled = false;

  ngAfterViewInit(): void {
    throw new Error('Method not implemented.');
  }

  updateView(): void {
    this.scrollBarRef.update();
  }

  scrollToBottom() {
    this.scrollBarRef.scrollTo({ bottom: 0 });
  }

  scrollToTop() {
    this.scrollBarRef.scrollTo({ top: 0 });
  }

  scrollTo(to: number) {
    this.scrollBarRef.scrollTo({ top: to });
  }
}
