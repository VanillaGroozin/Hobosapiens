using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mono.Data.Sqlite;
using System;

public static class DB
{
    private static string connectionString = $"URI=file:{Application.dataPath}/DB/HobosapiensDB";

    public static void GetDialogues()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select * from ialogues;";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log($"id {reader["id"]}");
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
    }

    public static List<DialogueLine> GetDialogueLinesBySpeakerId(int speakerId)
    {
        List<DialogueLine> dialogueLines = new List<DialogueLine>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    $"WITH RECURSIVE cte_dialogues (id, parent_id, speaker_id, text, button_text, same_childs_as_dialogue_id, effects, requirements) AS ( " +
                    $"SELECT d.id, d.parent_id, d.speaker_id, d.text, d.button_text, d.same_childs_as_dialogue_id, d.effects, d.requirements " +
                    $"FROM dialogues d " +
                    $"WHERE d.speaker_id = {speakerId} or d.speaker_id = 9999 " +
                    $"UNION ALL " +
                    $"SELECT d.id, d.parent_id, d.speaker_id, d.text, d.button_text, d.same_childs_as_dialogue_id, d.effects, d.requirements " +
                    $"FROM dialogues d " +
                    $"JOIN cte_dialogues c ON c.id = d.parent_id) " +
                    $"SELECT distinct c.*, s.name FROM cte_dialogues c " +
                    $"join speakers s on s.id = c.speaker_id " +
                    $"order by c.id;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dialogueLines.Add(new DialogueLine(
                            Convert.ToInt32(reader["id"]),
                            Convert.ToInt32(reader["parent_id"]),
                            Convert.ToInt32(reader["speaker_id"]),
                            reader["same_childs_as_dialogue_id"].ToString(),
                            reader["text"].ToString(),
                            reader["button_text"].ToString(),
                            reader["name"].ToString(),
                            reader["effects"].ToString(),
                            reader["requirements"].ToString()));
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        return dialogueLines;
    }


    public static List<DialogueLine> GetGeneralDialogueLines()
    {
        List<DialogueLine> dialogueLines = new List<DialogueLine>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    $"select * from generaldialogues";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dialogueLines.Add(new DialogueLine(
                            Convert.ToInt32(reader["id"]),
                            Convert.ToInt32(reader["parent_id"]),
                            0,
                            string.Empty,
                            reader["text"].ToString(),
                            string.Empty,
                            string.Empty,
                            reader["effects"].ToString(),
                            reader["requirements"].ToString(),
                            true));
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        return dialogueLines;
    }


    public static Quest GetQuestById(int questId)
    {
        Quest quest = new Quest();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    $"select * from Quests " +
                    $"where id = {questId} " +
                    $"limit 1; ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        quest = new Quest(Convert.ToInt32(reader["id"]),
                            reader["title"].ToString(),
                            reader["description"].ToString(),
                            Convert.ToInt32(reader["experience_reward"]),
                            Convert.ToInt32(reader["gold_reward"]));
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        return quest;
    }
}
