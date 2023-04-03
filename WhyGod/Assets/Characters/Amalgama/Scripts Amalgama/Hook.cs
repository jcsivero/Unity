using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] GameObject directionMouse;

    [SerializeField] LayerMask InteractuableHook;

    [SerializeField] float distance;

    [SerializeField] float anguloSubir;

    [SerializeField] float velocidadSubir = 0.1f;


    private RaycastHit2D hitInfo;
    
    private Vector3 hitPosition;

    private Vector2 actualizadoPoint;

    private SpringJoint2D hook;
    

    private LineRenderer brazos;
    private bool agarrado = false;

    private Rigidbody2D theBody;

    private GameObject LocalizacionAgarrado;
    [SerializeField] private GameObject ObjetoLocalizacionAgarrado;

    [Header("Animacion Hook")]

    [SerializeField] private float animationDuration = 1f;

    private bool animacionRealizada = false;

    private void Start()
    {
        hook = GetComponent<SpringJoint2D>();
        hook.enabled = false;
        brazos = GetComponent<LineRenderer>();
        brazos.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarBrazoRender();
        if (Input.GetButtonDown("Hook"))
        {
            HookAction();
        }
        if (Input.GetButtonUp("Hook"))
        {
            DejarAgarrar();
        }
    }

    private void ActualizarBrazoRender ()
    {
        if (agarrado)
        {
            brazos.enabled = true;
            // Actualizar agarre a la posicion actual
            hitPosition = LocalizacionAgarrado.transform.position;
            hook.connectedAnchor = new Vector2(hitPosition.x, hitPosition.y);
            // Actualizar los brazos a la posicion correspondiente
            brazos.SetPosition(0, transform.position);
            brazos.SetPosition(1, LocalizacionAgarrado.transform.position);
            if (!animacionRealizada)
            {
                animacionRealizada = true;
                StartCoroutine(AnimarBrazos());
            }
            // Calcular si hay un nuevo objeto de por medio
            actualizadoPoint = Physics2D.Raycast(transform.position, hitPosition - transform.position, distance, InteractuableHook).point;
            // Condiciones para dejar de agarrar por entorno
            if (transform.position.y > hitPosition.y  || // Si personaje est� por encima del enganche
                Mathf.Abs(hitPosition.x - actualizadoPoint.x) > 1f ||
                Mathf.Abs(hitPosition.y - actualizadoPoint.y) > 1f) { // Se atraviesa un nuevo objeto de por medio
                DejarAgarrar();
            }
            // Subir por el brazo
            if (Input.GetAxisRaw("Vertical") > anguloSubir) 
            {
                hook.distance -= velocidadSubir;   
            }
        }
    }

    private void DejarAgarrar ()
    {
        brazos.enabled = false;
        agarrado = false;
        hook.enabled = false;
        animacionRealizada = false;
        Destroy(LocalizacionAgarrado);
    }   

    private void HookAction()
    {
        Vector3 origin = transform.position;
        Vector3 direction = directionMouse.transform.up;
        hitInfo = Physics2D.Raycast(origin, direction, distance, InteractuableHook);
        if (hitInfo)
        {
            LocalizacionAgarrado = Instantiate(ObjetoLocalizacionAgarrado, hitInfo.point , new Quaternion(), hitInfo.collider.gameObject.transform);
            hitPosition = LocalizacionAgarrado.transform.position;
            hook.distance = hitInfo.distance;
            hook.connectedAnchor = new Vector2(hitPosition.x, hitPosition.y);
            hook.enabled = true;
            agarrado = true;
        }
    }

    private IEnumerator AnimarBrazos()
    {
        float startTime = Time.time;

        Vector3 startPosition = brazos.GetPosition(0);
        Vector3 endPosition = brazos.GetPosition(1);

        Vector3 pos = startPosition;
        while (pos != endPosition)
        {
            float t = (Time.time - startTime) / animationDuration;
            pos = Vector3.Lerp(startPosition, endPosition, t);
            brazos.SetPosition(1, pos);
            yield return null;
        }
    }
}
