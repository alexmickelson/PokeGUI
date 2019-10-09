using PokeGUI.Models;
using System.Collections.Generic;

namespace PokeGUI.Services
{
    public interface IPokemonExcelService
    {
        (string, PokeType) getStoredFilter();
        void generatePokemonExcelSheet(IEnumerable<Pokemon> pokemonCollection);
    }


}