import { Component } from '@angular/core';
import { CurrencyExchangeServiceServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-currencyexchange',
  templateUrl: './currencyexchange.component.html',
  styleUrls: ['./currencyexchange.component.css']
})
export class CurrencyexchangeComponent {
  fromCurrency = '';
  toCurrency = '';
  amount: number = 1;
  convertedAmount: number | null = null;
  loading = false;
  error = '';

  currencies: string[] = ['USD', 'INR', 'EUR', 'GBP', 'AUD', 'CAD', 'JPY', 'CNY'];

  constructor(private exchangeService: CurrencyExchangeServiceServiceProxy) {}

  convert() {
    this.loading = true;
    this.error = '';
    this.exchangeService
      .convertAmount(this.fromCurrency, this.toCurrency, this.amount)
      .subscribe({
        next: (result) => {
          this.convertedAmount = result;
          this.loading = false;
        },
        error: (err) => {
          this.error = err.message;
          this.loading = false;
        }
      });
  }
  
}
