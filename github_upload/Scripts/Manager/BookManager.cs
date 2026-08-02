using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public Book book;
    public BookInfomation[] infomations;
    public bool cover;
    public void BookNumber(int num)
    {
        if (book.answer_page != null)
        {
            book.answer_page = infomations[num].answerPage;
            book.answer_Number = infomations[num].answerPageNumbers.ToList();
        }
        if (cover)
        {
            book.background = infomations[num].bookPage[0];
            //book.bookPages = infomations[num].bookPage;
            book.bookPages = new Sprite[infomations[num].bookPage.Length - 1];
            Array.Copy(infomations[num].bookPage, 1, book.bookPages, 0, book.bookPages.Length);
        }
        else
        {
            book.bookPages = infomations[num].bookPage;
        }
        book.StartSet();
    }
}
