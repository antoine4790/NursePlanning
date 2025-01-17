﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebNursePlanning.Areas.Identity.Pages.Account.Manage
{
	public class ResetAuthenticatorModel : PageModel
	{
		private UserManager<Person> _userManager;
		private readonly SignInManager<Person> _signInManager;
		private ILogger<ResetAuthenticatorModel> _logger;

		public ResetAuthenticatorModel(
			UserManager<Person> userManager,
			SignInManager<Person> signInManager,
			ILogger<ResetAuthenticatorModel> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
		}

		[TempData]
		public string StatusMessage { get; set; }

		public async Task<IActionResult> OnGet()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			await _userManager.SetTwoFactorEnabledAsync(user, false);
			await _userManager.ResetAuthenticatorKeyAsync(user);
			_logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

			await _signInManager.RefreshSignInAsync(user);
			StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

			return RedirectToPage("./EnableAuthenticator");
		}
	}
}