using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Blog.Service.Extension
{
	public static class FluentValidationExtension
	{
		public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
		{
			foreach (var error in result.Errors)
			{
				modelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}
		}
		
		
	}
}
