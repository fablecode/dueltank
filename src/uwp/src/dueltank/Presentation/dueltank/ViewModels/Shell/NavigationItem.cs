using System;
using dueltank.ViewModels.Infrastructure.ViewModels;

namespace dueltank.ViewModels.Shell
{
    public class NavigationItem : ObservableObject
    {
        public readonly string FontFamily = "Segoe MDL2 Assets";
        public readonly string Glyph;
        public readonly string Label;
        public readonly Type ViewModel;

        private string _tag;


        public NavigationItem(Type viewModel)
        {
            ViewModel = viewModel;
        }

        public NavigationItem(int glyph, string label, Type viewModel) : this(viewModel)
        {
            Label = label;
            Glyph = char.ConvertFromUtf32(glyph);
        }

        public string Tag
        {
            get => _tag;
            set => Set(ref _tag, value);
        }
    }
}