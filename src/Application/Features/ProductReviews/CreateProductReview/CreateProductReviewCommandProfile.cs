﻿using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.ProductReviews.CreateProductReview
{
    internal class CreateProductReviewCommandProfile : Profile
    {
        public CreateProductReviewCommandProfile()
        {
            CreateMap<CreateProductReviewCommand, ProductReview>(MemberList.Source)
                .ForSourceMember(src => src.CommandId, opt => opt.DoNotValidate());
        }
    }
}