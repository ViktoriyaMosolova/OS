package com.example.myapp

import android.content.Context
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import androidx.activity.ComponentActivity
import java.util.Calendar

class MainActivity : ComponentActivity() {
    private lateinit var task1Button: Button
    private lateinit var task2Button: Button
    private lateinit var task3Button: Button
    private lateinit var EditText1: EditText
    private lateinit var EditText2: EditText
    private lateinit var EditText3: EditText
    private lateinit var resultTextView: TextView
    private var resetHandler: Handler? = null

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
        EditText1.setText(preferences.getString("taskText1", ""))
        EditText2.setText(preferences.getString("taskText2", ""))
        EditText3.setText(preferences.getString("taskText3", ""))
        task1Button.isEnabled = preferences.getBoolean("task1Enabled", true)
        task2Button.isEnabled = preferences.getBoolean("task2Enabled", true)
        task3Button.isEnabled = preferences.getBoolean("task3Enabled", true)
        resultTextView.isEnabled = preferences.getBoolean("resultTextView3Enabled", true)

        // Проверяем, прошел ли уже новый день
        val lastResetTime = preferences.getLong("lastResetTime", 0)
        val calendar = Calendar.getInstance()
        val currentTime = System.currentTimeMillis()
        calendar.timeInMillis = currentTime
        val currentDate = calendar.time.toString()
        val savedDate = preferences.getString("currentDate", "")
        if (currentDate != savedDate || lastResetTime == 0L) {
            // Очищаем сохраненные данные, так как уже новый день
            preferences.edit().clear().apply()
            preferences.edit().putLong("lastResetTime", currentTime).apply()
        } else {
            // Проверяем, если выполнены все задачи, выводим надпись "Супер мега умничка"
            checkAllTasksDone()
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


    override fun onResume() {
        super.onResume()

        // Запускаем задачу, которая будет сбрасывать состояние задач и кнопок каждый день в 10:32
        resetHandler?.postDelayed(resetTask, getTimeUntilMidnight())

        checkAllTasksDone()
    }

    override fun onPause() {
        super.onPause()

        // Отменяем задачу, чтобы не запускать ее, когда приложение свернуто
        resetHandler?.removeCallbacks(resetTask)
        resetHandler = null
    }

    override fun onStop() {
        super.onStop()

        // Сохраняем состояние задач и кнопок
        val preferences = getSharedPreferences("MyPreferences", Context.MODE_PRIVATE)
        preferences.edit()
            .putString("task1", task1Button.text.toString())
            .putString("task2", task2Button.text.toString())
            .putString("task3", task3Button.text.toString())
            .putString("taskText1", EditText1.text.toString())
            .putString("taskText2", EditText2.text.toString())
            .putString("taskText3", EditText3.text.toString())
            .putBoolean("task1Enabled", task1Button.isEnabled)
            .putBoolean("task2Enabled", task2Button.isEnabled)
            .putBoolean("task3Enabled", task3Button.isEnabled)
            .putBoolean("resultTextView3Enabled", resultTextView.isEnabled)
            .putLong("lastResetTime", System.currentTimeMillis())
            .putString("currentDate", Calendar.getInstance().time.toString())
            .apply()
    }

    private fun markTaskAsDone(button: Button) {
        if (button.isEnabled) {
            button.isEnabled = false
            button.text = "Умничка!"
            checkAllTasksDone() // проверяем, выполнены ли все задачи
        }
    }

    private fun checkAllTasksDone() {
        if (!task1Button.isEnabled && !task2Button.isEnabled && !task3Button.isEnabled) {
            resultTextView.text = "Супер мега умничка"
        }
    }

    private fun resetTasksAndButtons() {
        // Сбрасываем состояние задач и кнопок
        val preferences = getSharedPreferences("MyPreferences", Context.MODE_PRIVATE)
        preferences.edit()
            .putString("task1", "Выполнено")
            .putString("task2", "Выполнено")
            .putString("task3", "Выполнено")
            .putString("taskText1", "")
            .putString("taskText2", "")
            .putString("taskText3", "")
            .putBoolean("task1Enabled", true)
            .putBoolean("task2Enabled", true)
            .putBoolean("task3Enabled", true)
            .apply()

        // Обновляем UI
        runOnUiThread {
            task1Button.isEnabled = true
            task2Button.isEnabled = true
            task3Button.isEnabled = true
            task1Button.text = preferences.getString("task1", "Выполнено")
            task2Button.text = preferences.getString("task2", "Выполнено")
            task3Button.text = preferences.getString("task3", "Выполнено")
            EditText1.setText(preferences.getString("taskText1", ""))
            EditText2.setText(preferences.getString("taskText2", ""))
            EditText3.setText(preferences.getString("taskText3", ""))
            resultTextView.text = ""
        }

        // Устанавливаем время для следующего сброса состояния задач и кнопок
        resetHandler?.postDelayed(resetTask, getTimeUntilMidnight())
    }

    private fun getTimeUntilMidnight(): Long {
        val calendar = Calendar.getInstance()
        calendar.timeInMillis = System.currentTimeMillis()
        calendar.set(Calendar.HOUR_OF_DAY, 10)
        calendar.set(Calendar.MINUTE, 49)
        calendar.set(Calendar.SECOND, 0)
        calendar.set(Calendar.MILLISECOND, 0)
        val currentTime = System.currentTimeMillis()
        val resetTime = calendar.timeInMillis
        return if (resetTime <= currentTime) {
            resetTime + 24 * 60 * 60 * 1000 - currentTime
        } else {
            resetTime - currentTime
        }
    }


    private val resetTask = object : Runnable {
        override fun run() {
            resetTasksAndButtons()
            resetHandler?.postDelayed(this, getTimeUntilMidnight())
        }
    }
}