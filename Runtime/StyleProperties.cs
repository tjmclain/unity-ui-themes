using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[Serializable]
	public class AlphaProperty : StyleProperty<float>
	{
	}

	[Serializable]
	public class FontAssetProperty : StyleProperty<TMPro.TMP_FontAsset>
	{
	}

	[Serializable]
	public class FontStylesProperty : StyleProperty<TMPro.FontStyles>
	{
	}

	[Serializable]
	public class ImageTypeProperty : StyleProperty<ImageType>
	{
	}

	[Serializable]
	public class SpriteProperty : StyleProperty<Sprite>
	{
	}
}