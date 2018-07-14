# ThomasJepp.SaintsRow
A .NET library for Saints Row file formats and associated tools for Saints Row.

## Supported file formats
The ThomasJepp.SaintsRow library supports the following formats:

| File type | Extensions | Support |
|-----------|------------|---------|
| Red Faction Guerilla Package files (includes remake) | .vpp_pc, .str2_pc | Read only support |
| Saints Row 2 PC Package files | .vpp_pc | Full read and write support |
| Saints Row: The Third PC Package files | .vpp_pc, .str2_pc | Read only support |
| Saints Row: The Third PC Asset Assembler files | .asm_pc | Full read and write support (this is largely untested however!) |
| Saints Row IV and Saints Row: Gat out of Hell PC Package files | .vpp_pc, .str2_pc | Full read and write support |
| Saints Row IV and Saints Row: Gat out of Hell PC Asset Assembler files | .asm_pc | Full read and write support |
| Saints Row 2 PC Language Strings files | .le_strings | Full read and write support |
| Saints Row: The Third, Saints Row IV and Saints Row: Gat out of Hell Language strings files | .le_strings | Full read and write support |
| Saints Row: The Third, Saints Row IV and Saints Row: Gat out of Hell Streaming Soundbank files | ..._media.bnk_pc | Full read and write support |
| Saints Row: The Third, Saints Row IV and Saints Row: Gat out of Hell Wwise Soundbank files | .bnk_pc | Partial read only support |
| Saints Row: The Third, Saints Row IV and Saints Row: Gat out of Hell texture files | .cpeg_pc, .cvbm_pc | Partial read and write support (metadata only, no texture content) |
| Saints Row: The Third, Saints Row IV and Saints Row: Gat out of Hell static mesh files | .ccmesh_pc | Partial read and write support (metadata only, no model content) |
| Saints Row: The Third and Saints Row IV cloth simulation files | .sim_pc | Full read and write support |
| Saints Row: Gat out of Hell cloth simulation files | .sim_pc | Full read and write support |

## Licenses
All code is licensed under the license included in the repository as "license.txt".

A plain english explanation of what you need to do to comply with this license is included in "license_notes.txt".
