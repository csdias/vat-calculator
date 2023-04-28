﻿using MediatR;
using VatCalculator.Domain.Common;

namespace VatCalculator.Application.Queries.GetVatCalculationQuery;

public class GetVatCalculationRequestHandler 
            : IRequestHandler<GetVatCalculationRequest, Result<GetVatCalculationResponse>>
{

    public async Task<Result<GetVatCalculationResponse>> Handle(
                        GetVatCalculationRequest request,
                        CancellationToken cancellationToken)
    {

        var response = CalculateVat(request);

        return response;
    }

    public GetVatCalculationResponse CalculateVat(GetVatCalculationRequest request) =>
        request switch
        {
            { PriceWithoutVat: > 0 }
                    => new GetVatCalculationResponse()
                    {   
                        VatRate = request.VatRate,
                        PriceWithoutVat = request.PriceWithoutVat,
                        PriceWithVat = - ( - request.PriceWithoutVat - (request.PriceWithoutVat * (request.VatRate/100))),
                        Vat = -(-request.PriceWithoutVat - (request.PriceWithoutVat * (request.VatRate / 100))) - request.PriceWithoutVat,
                    },

            { Vat: > 0 }
                    => new GetVatCalculationResponse()
                    {
                        VatRate = request.VatRate,
                        Vat = request.Vat,
                        PriceWithoutVat = request.Vat / (request.VatRate / 100),
                        PriceWithVat = (request.Vat / (request.VatRate / 100)) + request.Vat
                    },

            { PriceWithVat: > 0 }
                    => new GetVatCalculationResponse()
                    {
                        VatRate = request.VatRate,
                        PriceWithVat = request.PriceWithVat,
                        PriceWithoutVat = request.PriceWithVat / (1 + (request.VatRate / 100)),
                        Vat = (request.PriceWithVat / (1 + (request.VatRate / 100)) ) * (request.VatRate / 100)
                    },

            { } => throw new Exception("Couldn't calculate on this input"),

            null => throw new ArgumentNullException(nameof(request), "Couldn't calculate on null input")
        };
}
