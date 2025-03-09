using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class Ansatte
{
    public int Brukerid { get; set; }

    public string Fornavn { get; set; } = null!;

    public string Etternavn { get; set; } = null!;

    public string Epost { get; set; } = null!;

    public string Passord { get; set; } = null!;

    public bool ErAdmin { get; set; }

    public bool Aktiv { get; set; }

    public DateTime Datoregistrert { get; set; }

    public virtual ICollection<BProsjekt> BProsjektAnsvarligansatt2s { get; set; } = new List<BProsjekt>();

    public virtual ICollection<BProsjekt> BProsjektAnsvarligansatt3s { get; set; } = new List<BProsjekt>();

    public virtual ICollection<BProsjekt> BProsjektAnsvarligansatt4s { get; set; } = new List<BProsjekt>();

    public virtual ICollection<BProsjekt> BProsjektAnsvarligansatt5s { get; set; } = new List<BProsjekt>();

    public virtual ICollection<BProsjekt> BProsjektAnsvarligansatts { get; set; } = new List<BProsjekt>();

    public virtual ICollection<BProvetakingsplan> BProvetakingsplanPlanlegger2s { get; set; } = new List<BProvetakingsplan>();

    public virtual ICollection<BProvetakingsplan> BProvetakingsplanPlanleggers { get; set; } = new List<BProvetakingsplan>();

    public virtual ICollection<BRapport> BRapportGenerertavs { get; set; } = new List<BRapport>();

    public virtual ICollection<BRapport> BRapportGodkjentavs { get; set; } = new List<BRapport>();

    public virtual ICollection<Endringslogg> Endringsloggs { get; set; } = new List<Endringslogg>();
}
