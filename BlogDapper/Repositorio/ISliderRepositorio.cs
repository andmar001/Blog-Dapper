using BlogDapper.Models;

namespace BlogDapper.Repositorio
{
    public interface ISliderRepositorio
    {
        Slider GetSlider(int id);
        List<Slider> GetSliders();
        Slider CrearSlider(Slider slider);
        Slider ActualizarSlider(Slider slider);
        void BorrarSlider(int id);
    }
}
