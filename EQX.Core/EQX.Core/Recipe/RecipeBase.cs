using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;

namespace EQX.Core.Recipe
{
    public class RecipeBase : ObservableObject, IRecipe
    {
        public event RecipeChangedEventHandler? RecipeChanged;

        public int Id { get; internal set; }

        public string Name { get; set; }

        public virtual IRecipe Load()
        {
            return new RecipeBase();
        }

        public virtual void Save(){}

        protected void OnRecipeChanged(object oldValue, object newValue, [CallerMemberName] string? propertyName = null)
        {
            RecipeChanged?.Invoke(oldValue, newValue, propertyName);
        }
    }
}
