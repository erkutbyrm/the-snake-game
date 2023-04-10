using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeController : MonoBehaviour
{
    private Vector2 _sceneLimit = new Vector2(34, 19);
    [SerializeField] private GameObject food;
    [SerializeField] private GameObject tailPrefab;
    [SerializeField] private float speed = (float)0.15;
    [SerializeField] private TextMeshPro textScore;
    [SerializeField] private TextMeshPro textEnd;
    private Vector2 _direction = Vector2.down;
    private List<Transform> _snake = new List<Transform>();
    private int _score;
    private bool _grow;
    private Vector2 _nexDir;

    private int Score 
    {
        get => _score;

        set {
            _score = value;
            textScore.text = _score.ToString();
        }
    }

    private void Start()
    {
        Score = 0;
        _nexDir = Vector2.down;
        textEnd.enabled = false;
        RegenFood();
        StartCoroutine(Move());
        _snake.Add(transform);
    }


    private void Update()
    {
        if ( (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ) && _direction != Vector2.down){
            _nexDir = Vector2.up;
        }
        if ( (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && _direction != Vector2.right){
            _nexDir = Vector2.left;
        }
        if ( (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && _direction != Vector2.up){
            _nexDir = Vector2.down;
        }
        if ( (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && _direction != Vector2.left){
            _nexDir = Vector2.right;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Food")){
            _grow = true;
        }
        if (other.CompareTag("Wall")){
            Dead();
        }
    }
    
    private bool isSpawnable(Vector3 pos) {
        foreach (var item in _snake)
        {
            if(item.position == pos) {
                return false;
            }
        }
        return true;
    }

    private void RegenFood(){
        Vector3 newFoodPos;
        do
        {
            var x = (int) Random.Range(1,_sceneLimit.x);
            var y = (int) Random.Range(1, _sceneLimit.y);
            newFoodPos = new Vector3(x,y,0);
        } while ( ! isSpawnable(newFoodPos));

        food.transform.position = newFoodPos;
    }

    private void Grow(){
        Score++;
        var tail = Instantiate(tailPrefab);
        _snake.Add(tail.transform);
        //tail.transform.position = _snake[_snake.Count-1].position
        _snake[_snake.Count-1].position = _snake[_snake.Count-2].position;

        RegenFood();

    }

    private void Dead(){
        textEnd.enabled = true;
        StopAllCoroutines();
    }    
    
    private IEnumerator Move() {
        while (true) {
            if (_grow) {
                _grow = false;
                Grow();
            }

            for (int i = _snake.Count-1; i > 0; i--)
            {
                _snake[i].position = _snake[i-1].position;
            }

            _direction = _nexDir;
            var position = transform.position;
            position += (Vector3)_direction;
            transform.position = position;
            yield return new WaitForSeconds(speed);

            
        }
    }
}
