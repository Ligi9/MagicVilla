﻿using MagicVilla_API.Modelo.Dto;

namespace MagicVilla_API.Datos;

public class VillaStore
{
    public static List<VillaDto> villaList = new List<VillaDto>
    {
      new VillaDto{Id = 1, Nombre = "Vista a la Piscino", Ocupantes=3, MetrosCuadrados=50},
      new VillaDto{Id = 2, Nombre = "Vista a la Playa", Ocupantes=4, MetrosCuadrados=80},
    };
}
