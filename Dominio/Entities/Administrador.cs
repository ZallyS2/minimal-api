using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Dominio.Entities;

public class Administrador {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string Perfil { get; set; } = default!;

    [Required]
    [StringLength(250)]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Password { get; set; } = default!;

}