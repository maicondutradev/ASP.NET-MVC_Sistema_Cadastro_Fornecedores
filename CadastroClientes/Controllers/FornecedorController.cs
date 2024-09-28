using CadastroClientes.Models;
using CadastroClientes.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class FornecedorController : Controller
{
    private readonly IFornecedorRepository _fornecedorRepository;

    public FornecedorController(IFornecedorRepository fornecedorRepository)
    {
        _fornecedorRepository = fornecedorRepository;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        var fornecedores = await _fornecedorRepository.GetAllAsync(searchString);
        return View(fornecedores);
    }

    public async Task<IActionResult> Details(int? id)
    {
        var fornecedor = await _fornecedorRepository.GetByIdAsync(id);
        if (fornecedor == null) return NotFound("Fornecedor não encontrado!");
        return View(fornecedor);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("ID,Nome,CNPJ,Segmento,CEP,Endereco,FotoUsuario")] Fornecedor fornecedor)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(fornecedor.CEP))
            {
                var endereco = await BuscarEnderecoPorCep(fornecedor.CEP);
                if (endereco != null)
                {
                    fornecedor.Endereco = $"{endereco.Logradouro}, {endereco.Bairro}, {endereco.Localidade} - {endereco.Uf}";
                }
                else
                {
                    ModelState.AddModelError("CEP", "Não foi possível buscar o endereço para o CEP fornecido.");
                    return View(fornecedor);
                }
            }

            if (fornecedor.FotoUsuario != null && fornecedor.FotoUsuario.Length > 0)
            {
                var mimeType = fornecedor.FotoUsuario.ContentType;
                if (mimeType != "image/png")
                {
                    ModelState.AddModelError("FotoUsuario", "Apenas arquivos .png são permitidos!");
                    return View(fornecedor);
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + fornecedor.FotoUsuario.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fornecedor.FotoUsuario.CopyToAsync(fileStream);
                }

                fornecedor.FotoUsuarioPath = "/uploads/" + uniqueFileName;
            }

            await _fornecedorRepository.AddAsync(fornecedor);
            TempData["SuccessMessage"] = "Fornecedor criado com sucesso!";
            return RedirectToAction(nameof(Details), new {id = fornecedor.ID});
        }

        return View(fornecedor);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        var fornecedor = await _fornecedorRepository.GetByIdAsync(id);
        if (fornecedor == null) return NotFound("Fornecedor não encontrado!");
        return View(fornecedor);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,CNPJ,Segmento,CEP,Endereco,FotoUsuario")] Fornecedor fornecedor)
    {
        if (id != fornecedor.ID) return NotFound();

        if (ModelState.IsValid)
        {
            if (fornecedor.FotoUsuario != null && fornecedor.FotoUsuario.Length > 0)
            {
                var mimeType = fornecedor.FotoUsuario.ContentType;
                if (mimeType != "image/png")
                {
                    ModelState.AddModelError("FotoUsuario", "Apenas arquivos .png são permitidos!");
                    return View(fornecedor);
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + fornecedor.FotoUsuario.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fornecedor.FotoUsuario.CopyToAsync(fileStream);
                }

                fornecedor.FotoUsuarioPath = "/uploads/" + uniqueFileName;
            }

            await _fornecedorRepository.UpdateAsync(fornecedor);
            TempData["SuccessMessage"] = "Fornecedor atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        return View(fornecedor);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        var fornecedor = await _fornecedorRepository.GetByIdAsync(id);
        if (fornecedor == null) return NotFound("Fornecedor não encontrado!");
        return View(fornecedor);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _fornecedorRepository.DeleteAsync(id);
        TempData["SuccessMessage"] = "Fornecedor excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }
    private async Task<ViaCepResponse> BuscarEnderecoPorCep(string cep)
    {
        using (var httpClient = new HttpClient())
        {
            var url = $"https://viacep.com.br/ws/{cep}/json/";

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ViaCepResponse>(json);
            }
            else
            {
                return null;
            }
        }
    }
}
