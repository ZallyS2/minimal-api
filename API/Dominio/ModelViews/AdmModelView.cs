namespace MinimalApi.Dominio.ModelViews {
    public record AdmModelView {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
    }
}