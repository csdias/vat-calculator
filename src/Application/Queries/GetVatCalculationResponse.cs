﻿namespace VatCalculator.Application.Queries.GetVatCalculationQuery;

public class GetVatCalculationResponse
{
    public decimal VatRate { get; set; }

    public decimal PriceWithoutVat { get; set; }

    public decimal Vat { get; set; }

    public decimal PriceWithVat { get; set; }

}