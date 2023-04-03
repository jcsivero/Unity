using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
  private Rigidbody2D rb2D;

	[Header("Movimiento")]

	private float movimientoHorizontal = 0f;
	[SerializeField] private float velocidadMovimiento;
	[SerializeField] private float suavizadoMovimiento;
	private Vector3 velocidad = Vector3.zero;
	private bool mirandoDerecha = true;


	[Header("Salto")]

	[SerializeField] private float fuerzaSalto;
	[SerializeField] private LayerMask Suelo;
	[SerializeField] private Transform controladorSuelo;
	[SerializeField] private Vector3 dimensionesCaja;
	[SerializeField] private bool enSuelo;
	private bool saltar;
	
	[Range(0,1)] [SerializeField] private float multCancelarSalto;
	[SerializeField] private float multGravedad;
	private bool botonSaltoPulsado = true;

	[Header("Dash")]

	[SerializeField] private float dashTime;
	[SerializeField] private float speedDash;
	[SerializeField] private float dashCooldown;
	private float initialGravity;
	private bool canDash = true;
	private bool canMove = true;


	private void Start() {
		rb2D = GetComponent<Rigidbody2D>();
		initialGravity = rb2D.gravityScale;
	}

	private void Update() {

		movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadMovimiento;

		if (Input.GetButton("Jump")) { saltar = true; }
		if (Input.GetButtonUp("Jump")) { SaltarSinPulsar(); }

		if (Input.GetButtonDown("Dash") && canDash) { StartCoroutine(Dash()); }
	}

	private void FixedUpdate() {
		enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, Suelo);
		// mecanismo movimiento	
		if (canMove) { Mover(movimientoHorizontal * Time.deltaTime); }
		saltar = false;
	}	

	private void Mover(float mover) {
		Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
		// MOVIMIENTO
		rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoMovimiento);
		// GIRAR
		if ((mover > 0 && !mirandoDerecha) || (mover < 0 && mirandoDerecha)) { Girar(); }// Girar Derecha 
		// SALTAR
		if (enSuelo && saltar && botonSaltoPulsado) { Saltar(); }

		if (rb2D.velocity.y < 0 && !enSuelo) { rb2D.gravityScale = initialGravity * multGravedad; }
		else { rb2D.gravityScale = initialGravity; }
	}

	private void Saltar()
	{
		rb2D.AddForce(Vector2.up * fuerzaSalto);
		enSuelo = false;
		saltar = false;
		botonSaltoPulsado = false;
	}

	private void SaltarSinPulsar()
	{
		if (rb2D.velocity.y > 0) { rb2D.AddForce(Vector2.down * rb2D.velocity.y * (1 - multCancelarSalto), ForceMode2D.Impulse); }
		botonSaltoPulsado = true;
		saltar = false;
	}

	private IEnumerator Dash()
    {
		canMove = false;
		canDash = false;
		rb2D.gravityScale = 0f;
		float H = Input.GetAxisRaw("Horizontal");
		float V = Input.GetAxisRaw("Vertical");
		if (V > 0) { V = 0; }
		if (Mathf.Abs(H) + Mathf.Abs(V) == 0f) { rb2D.velocity = new Vector2(speedDash * transform.localScale.x, 0f); }
		else { rb2D.velocity = new Vector2(speedDash * H, speedDash * V); }
		yield return new WaitForSeconds(dashTime);
		canMove = true;
		rb2D.gravityScale = initialGravity;
		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
    }

	private void Girar() {
		mirandoDerecha = !mirandoDerecha;
		Vector3 escala = transform.localScale; 
		escala.x *= -1;
		transform.localScale = escala;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
	}
}