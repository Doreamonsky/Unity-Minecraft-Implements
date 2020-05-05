Shader "Toon/Lit Outline Camo" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 0.05)) = .03
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 

		_CamoBlackTint("Camo Pattern Black Tint", Color) = (0.41, 0.41, 0.21, 1.0)
		_CamoRedTint("Camo Pattern Red Tint", Color) = (0.19, 0.20, 0.13, 1.0)
		_CamoGreenTint("Camo Pattern Green Tint", Color) = (0.75, 0.64, 0.31, 1.0)
		_CamoBlueTint("Camo Pattern Blue Tint", Color) = (0.34, 0.23, 0.10, 1.0)
		_CamoPatternMap("Camo Pattern (RGB)", 2D) = "black" {}
		[KeywordEnum(UV1, UV2)] _UV_CHANNEL("Pattern UV-Channel", Float) = 0
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		UsePass "Toon/Cammo/FORWARD"
		UsePass "Toon/Basic Outline/OUTLINE"
	} 

	Fallback "Toon/Lit"
}
