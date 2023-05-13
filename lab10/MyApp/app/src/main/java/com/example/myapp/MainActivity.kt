package com.example.myapp

import android.os.Bundle
import androidx.activity.ComponentActivity
import android.widget.Button
import android.widget.TextView
import android.widget.EditText
import android.content.Context
import java.util.Calendar
import android.text.Editable

class MainActivity : ComponentActivity() {
    private lateinit var task1Button: Button
    private lateinit var task2Button: Button
    private lateinit var task3Button: Button
    private lateinit var EditText1: EditText
    private lateinit var EditText2: EditText
    private lateinit var EditText3: EditText
    private lateinit var resultTextView: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        task1Button = findViewById(R.id.task1_button)
        task2Button = findViewById(R.id.task2_button)
        task3Button = findViewById(R.id.task3_button)
        EditText1 = findViewById(R.id.task1_edittext)
        EditText2 = findViewById(R.id.task2_edittext)
        EditText3 = findViewById(R.id.task3_edittext)
        resultTextView = findViewById(R.id.result_textview)

        // Восстанавливаем сохраненные значения кнопок и текстового поля
        val preferences = getSharedPreferences("MyPreferences", Context.MODE_PRIVATE)
        task1Button.text = preferences.getString("task1", "Выполнено")
        task2Button.text = preferences.getString("task2", "Выполнено")
        task3Button.text = preferences.getString("task3", "Выполнено")
        EditText1.text = Editable.Factory.getInstance().newEditable(preferences.getString("taskText1", ""))
        EditText2.text = Editable.Factory.getInstance().newEditable(preferences.getString("taskText2", ""))
        EditText3.text = Editable.Factory.getInstance().newEditable(preferences.getString("taskText3", ""))
        task1Button.isEnabled = preferences.getBoolean("task1Enabled", true)
        task2Button.isEnabled = preferences.getBoolean("task2Enabled", true)
        task3Button.isEnabled = preferences.getBoolean("task3Enabled", true)
        resultTextView.isEnabled = preferences.getBoolean("resultTextView3Enabled", true)

        // Проверяем, прошел ли уже новый день
        val calendar = Calendar.getInstance()
        val currentTime = System.currentTimeMillis()
        calendar.timeInMillis = currentTime
        val currentDate = calendar.time.toString()
        val savedDate = preferences.getString("currentDate", "")
        if (currentDate != savedDate) {
            // Очищаем сохраненные данные, так как уже новый день
            preferences.edit().remove("task1").remove("task2").remove("task3")
                .remove("taskText").remove("task1Enabled").remove("task2Enabled")
                .remove("task3Enabled").remove("resultTextView3Enabled").apply()
        }

        task1Button.setOnClickListener {
            markTaskAsDone(task1Button)
        }
        task2Button.setOnClickListener {
            markTaskAsDone(task2Button)
        }
        task3Button.setOnClickListener {
            markTaskAsDone(task3Button)
        }
    }
    override fun onStop() {
        super.onStop()

        val preferences = getSharedPreferences("MyPreferences", Context.MODE_PRIVATE)
        val editor = preferences.edit()

        editor.putString("task1", task1Button.text.toString())
        editor.putString("task2", task2Button.text.toString())
        editor.putString("task3", task3Button.text.toString())
        editor.putString("taskText1", EditText1.text.toString())
        editor.putString("taskText2", EditText2.text.toString())
        editor.putString("taskText3", EditText3.text.toString())
        editor.putBoolean("task1Enabled", task1Button.isEnabled)
        editor.putBoolean("task2Enabled", task2Button.isEnabled)
        editor.putBoolean("task3Enabled", task3Button.isEnabled)
        editor.putBoolean("resultTextView3Enabled", task3Button.isEnabled)

        val calendar = Calendar.getInstance()
        val currentTime = System.currentTimeMillis()
        calendar.timeInMillis = currentTime
        val currentDate = calendar.time.toString()
        editor.putString("currentDate", currentDate)

        calendar.set(Calendar.HOUR_OF_DAY, 10)
        calendar.set(Calendar.MINUTE, 0)
        calendar.set(Calendar.SECOND, 0)
        calendar.set(Calendar.MILLISECOND, 0)
        val midnight = calendar.timeInMillis
        editor.putLong("lastUpdateTime", midnight)
        editor.apply()
    }
    private fun markTaskAsDone(button: Button) {
        if (button.isEnabled) {
            button.isEnabled = false
            button.text = "Умничка!"
            checkAllTasksDone()
        }
    }

    private fun checkAllTasksDone() {
        if (!task1Button.isEnabled && !task2Button.isEnabled && !task3Button.isEnabled) {
            resultTextView.text = "Супер мега умничка!"
        }
    }

}
