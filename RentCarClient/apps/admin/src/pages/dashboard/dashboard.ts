import { AfterViewInit, ChangeDetectionStrategy, Component, computed, ElementRef, inject, OnInit, signal, viewChild, ViewEncapsulation } from '@angular/core';
import { BreadcrumbService } from '../../services/breadcrumb';
import Blank from '../../components/blank/blank';
import { httpResource } from '@angular/common/http';
import { Result } from '../../models/result.model';
import Loading from '../../components/loading/loading';
import { Chart, ChartConfiguration, ChartType, registerables } from 'chart.js';

Chart.register(...registerables);

interface ChartData {
  data: any[];
  borderColor: string[];
}

@Component({
  imports: [
    Blank,
    Loading
  ],
  templateUrl: './dashboard.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Dashboard implements OnInit, AfterViewInit {
  readonly activeReservationCountResult = httpResource<Result<number>>(() => "/rent/dashboard/active-reservation-count");
  readonly activeReservationCount = computed(() => this.activeReservationCountResult.value()?.data ?? 0);
  readonly activeReservationCountLoading = computed(() => this.activeReservationCountResult.isLoading());

  readonly chartCanvas1 = viewChild.required<ElementRef<HTMLCanvasElement>>('revenueChartCanvas');
  readonly chartCanvas2 = viewChild.required<ElementRef<HTMLCanvasElement>>('reservationStatusChartCanvas');

  readonly chart1 = signal<Chart | null>(null);
  readonly chart2 = signal<Chart | null>(null);

  readonly #breadcrumb = inject(BreadcrumbService);

  readonly res1 = signal<ChartData>({
    data: [
      { date: '01.07.2025', total: 150.50 },
      { date: '02.07.2025', total: 200.75 },
      { date: '03.07.2025', total: 175.25 },
      { date: '04.07.2025', total: 220.00 },
      { date: '05.07.2025', total: 180.80 },
      { date: '06.07.2025', total: 300.45 },
      { date: '07.07.2025', total: 250.30 }
    ],
    borderColor: [
      'rgba(54, 162, 235, 1)',
      'rgba(255, 99, 132, 1)',
      'rgba(255, 205, 86, 1)',
      'rgba(75, 192, 192, 1)',
      'rgba(153, 102, 255, 1)',
      'rgba(255, 159, 64, 1)',
      'rgba(199, 199, 199, 1)'
    ]
  });
  readonly res2 = signal<ChartData>({
    data: [
      { date: '01.07.2025', total: 5 },
      { date: '02.07.2025', total: 10 },
      { date: '03.07.2025', total: 20 },
      { date: '04.07.2025', total: 30 },
      { date: '05.07.2025', total: 4 },
      { date: '06.07.2025', total: 50 },
      { date: '07.07.2025', total: 8 }
    ],
    borderColor: [
      'rgba(54, 162, 235, 1)',
      'rgba(255, 99, 132, 1)',
      'rgba(255, 205, 86, 1)',
      'rgba(75, 192, 192, 1)',
      'rgba(153, 102, 255, 1)',
      'rgba(255, 159, 64, 1)',
      'rgba(199, 199, 199, 1)'
    ]
  });

  ngOnInit(): void {
    this.#breadcrumb.setDashboard();
  }

  ngAfterViewInit(): void {
    this.createChart(this.chart1(), this.chartCanvas1(),'bar', this.res1(), "Günlük Kazanç Dağılımı (₺)", "Haftalık Kazanç Dağılımı");
    this.createChart(this.chart2(), this.chartCanvas2(),'doughnut', this.res2(),"Günlük Rezervasyon", "Haftalık Rezervasyon", "");
  }

  createChart(
    chart: Chart | null,
    canvas: any,
    type: ChartType,
    res: ChartData,
    label: string,
    text: string,
    symbol: string = ' ₺') {
    if (chart) {
      chart.destroy();
    }

    const ctx = canvas.nativeElement.getContext('2d');
    if (!ctx) return;

    const config: ChartConfiguration = {
      type: type,
      data: {
        labels: res.data.map(item => item.date),
        datasets: [{
          label: label,
          data: res.data.map(item => item.total),
          backgroundColor:res.borderColor,
          borderColor: res.borderColor,
          borderWidth: 2,
          fill: type === 'line' ? false : true
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          title: {
            display: true,
            text: text
          },
          legend: {
            display: true,
            position: 'top'
          }
        },
        scales: type !== 'doughnut' ? {
          y: {
            beginAtZero: true,
            ticks: {
              callback: function (value) {
                return value + symbol;
              }
            }
          }
        } : {}
      }
    };

    chart = new Chart(ctx, config);
  }
}
