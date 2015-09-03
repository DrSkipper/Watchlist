using UnityEngine;

/**
 * As recommended by Asher Vollmer https://twitter.com/AsherVo/status/461579941159501824
 */
public class VoBehavior : MonoBehaviour
{
	public new GameObject gameObject { get { 
        if (!_gameObject) _gameObject = base.gameObject;
        return _gameObject; 
    } }

	public new Transform transform { get {
			if (!_transform) _transform = base.transform;
			return _transform;
	} }

	public new Renderer renderer { get {
			if (!_renderer) _renderer = base.GetComponent<Renderer>();
			return _renderer;
	} }
	
	public SpriteRenderer spriteRenderer { get {
			if (!_spriteRenderer) _spriteRenderer = base.GetComponent<SpriteRenderer>();
			return _spriteRenderer;
	} }

    public BoxCollider2D boxCollider2D { get {
            if (!_boxCollider2D) _boxCollider2D = base.GetComponent<BoxCollider2D>();
            return _boxCollider2D;
    } }

    /**
     * Private
     */
    private GameObject _gameObject;
    private Transform _transform;
    private Renderer _renderer;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
}
