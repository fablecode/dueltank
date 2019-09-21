using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using Windows.System;
using dueltank.ViewModels.Infrastructure.ViewModels;
using Reactive.Bindings;

namespace dueltank.ViewModels.Dialogs
{
    public class UsernameContentDialogViewModel : ViewModelBase
    {
        //private string _username;
        //private string _isPrimaryButtonEnabled;


        //public string Username
        //{
        //    get => _username;
        //    set => Set(ref _username, value);
        //}
        //public string IsPrimaryButtonEnabled
        //{
        //    get => _isPrimaryButtonEnabled;
        //    set => Set(ref _isPrimaryButtonEnabled, value);
        //}

        [Required(ErrorMessage = "The username is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [RegularExpression(@"(^[\w]+$)", ErrorMessage = "Only letters and numbers")]
        public ReactiveProperty<string> Username { get; }

        public ReactiveProperty<string> UsernameErrorMessage { get; set; }

        public ReactiveProperty<bool> FormHasErrors { get; }


        public UsernameContentDialogViewModel()
        {
            Username = new ReactiveProperty<string>(mode: ReactivePropertyMode.Default | ReactivePropertyMode.IgnoreInitialValidationError)
                .SetValidateAttribute(() => Username);

            // You can combine some ObserveHasErrors values.
            FormHasErrors = new[]
                {
                    Username.ObserveHasErrors,
                }.CombineLatest(x => !x.Any(y => y))
                .ToReactiveProperty();

            UsernameErrorMessage = Username.ObserveErrorChanged
                .Select(x => x?.OfType<string>()?.FirstOrDefault())
                .ToReactiveProperty();
        }
    }
}