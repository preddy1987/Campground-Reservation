SET XACT_ABORT ON;

BEGIN TRAN;

IF OBJECT_ID('dbo.results', 'U') IS NOT NULL DROP TABLE results;
IF OBJECT_ID('dbo.answers', 'U') IS NOT NULL DROP TABLE answers;
IF OBJECT_ID('dbo.questions', 'U') IS NOT NULL DROP TABLE questions;

CREATE TABLE questions 
(
    id				INT				NOT NULL IDENTITY(1,1),
    question_text	VARCHAR(255)	NOT NULL,

	constraint pk_questions PRIMARY KEY (id)
);

CREATE TABLE answers 
(
    id			INT not null identity(1,1),
    question_id INT not null,
    answer_text VARCHAR(255) NOT NULL,
    is_correct	BIT NOT NULL DEFAULT 0,

	constraint pk_answers primary key (id),
	constraint fk_answers_questions FOREIGN KEY (question_id) REFERENCES questions(id)
);

CREATE TABLE results 
(
    id			INT				IDENTITY(1,1),
    name		VARCHAR(255)	NOT NULL,
    total_questions INT			NOT NULL,
    correct_questions INT		NOT NULL,

	constraint pk_results primary key (id)
);

INSERT INTO questions (question_text) VALUES ('What color is the sky?');
INSERT INTO questions (question_text) VALUES ('What''s in my pocket?');
INSERT INTO questions (question_text) VALUES ('What walks on four legs in morning, two at noon, and three in the evening?');
INSERT INTO questions (question_text) VALUES ('Mac or PC?');
INSERT INTO questions (question_text) VALUES ('Foster''s is Australian for what?');

INSERT INTO answers (question_id, answer_text, is_correct) VALUES (1, 'Yellow', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (1, 'Red', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (1, 'Blue', 1);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (1, 'Orange', 0);

INSERT INTO answers (question_id, answer_text, is_correct) VALUES (2, 'Filthy Hobbites', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (2, 'Ring', 1);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (2, 'Fish', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (2, 'Hole', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (2, 'Is it juicy?', 0);

INSERT INTO answers (question_id, answer_text, is_correct) VALUES (3, 'Filthy Hobbites', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (3, 'Gorilla', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (3, 'Fish', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (3, 'Mole', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (3, 'Man', 1);

INSERT INTO answers (question_id, answer_text, is_correct) VALUES (4, 'Mac', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (4, 'PC', 1);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (4, 'Linux', 0);

INSERT INTO answers (question_id, answer_text, is_correct) VALUES (5, 'Drop Bear', 0);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (5, 'Beer', 1);
INSERT INTO answers (question_id, answer_text, is_correct) VALUES (5, 'Steve', 0);

INSERT INTO results (name, total_questions, correct_questions) VALUES ('Steve Jobs', 5, 5);
INSERT INTO results (name, total_questions, correct_questions) VALUES ('Steve Wozniak', 5, 3);
INSERT INTO results (name, total_questions, correct_questions) VALUES ('Grace Hopper', 5, 5);
INSERT INTO results (name, total_questions, correct_questions) VALUES ('Ada Lovelace', 4, 5);
INSERT INTO results (name, total_questions, correct_questions) VALUES ('Sergey Brin', 1, 5);

COMMIT TRANSACTION;