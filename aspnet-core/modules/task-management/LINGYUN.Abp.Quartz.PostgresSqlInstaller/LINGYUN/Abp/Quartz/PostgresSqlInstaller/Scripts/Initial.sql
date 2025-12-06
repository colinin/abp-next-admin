-- This script is for PostgreSQL

-- This initializes the database to pristine for Quartz, by first removing any existing Quartz tables
-- and then recreating them from scratch.
-- Should you only require it to create the tables, set DropDb to 0.

SET client_min_messages = WARNING;
DROP TABLE IF EXISTS ${TablePrefix}fired_triggers;
DROP TABLE IF EXISTS ${TablePrefix}paused_trigger_grps;
DROP TABLE IF EXISTS ${TablePrefix}scheduler_state;
DROP TABLE IF EXISTS ${TablePrefix}locks;
DROP TABLE IF EXISTS ${TablePrefix}simprop_triggers;
DROP TABLE IF EXISTS ${TablePrefix}simple_triggers;
DROP TABLE IF EXISTS ${TablePrefix}cron_triggers;
DROP TABLE IF EXISTS ${TablePrefix}blob_triggers;
DROP TABLE IF EXISTS ${TablePrefix}triggers;
DROP TABLE IF EXISTS ${TablePrefix}job_details;
DROP TABLE IF EXISTS ${TablePrefix}calendars;
SET client_min_messages = NOTICE;

CREATE TABLE ${TablePrefix}job_details
  (
    sched_name TEXT NOT NULL,
    job_name TEXT NOT NULL,
    job_group TEXT NOT NULL,
    description TEXT NULL,
    job_class_name TEXT NOT NULL,
    is_durable BOOL NOT NULL,
    is_nonconcurrent BOOL NOT NULL,
    is_update_data BOOL NOT NULL,
    requests_recovery BOOL NOT NULL,
    job_data BYTEA NULL,
    PRIMARY KEY (sched_name, job_name, job_group)
);

CREATE TABLE ${TablePrefix}triggers
  (
    sched_name TEXT NOT NULL,
    trigger_name TEXT NOT NULL,
    trigger_group TEXT NOT NULL,
    job_name TEXT NOT NULL,
    job_group TEXT NOT NULL,
    description TEXT NULL,
    next_fire_time BIGINT NULL,
    prev_fire_time BIGINT NULL,
    priority INTEGER NULL,
    trigger_state TEXT NOT NULL,
    trigger_type TEXT NOT NULL,
    start_time BIGINT NOT NULL,
    end_time BIGINT NULL,
    calendar_name TEXT NULL,
    misfire_instr SMALLINT NULL,
    job_data BYTEA NULL,
    PRIMARY KEY (sched_name, trigger_name, trigger_group),
    FOREIGN KEY (sched_name, job_name, job_group)
      REFERENCES ${TablePrefix}job_details (sched_name, job_name, job_group)
);

CREATE TABLE ${TablePrefix}simple_triggers
  (
    sched_name TEXT NOT NULL,
    trigger_name TEXT NOT NULL,
    trigger_group TEXT NOT NULL,
    repeat_count BIGINT NOT NULL,
    repeat_interval BIGINT NOT NULL,
    times_triggered BIGINT NOT NULL,
    PRIMARY KEY (sched_name, trigger_name, trigger_group),
    FOREIGN KEY (sched_name, trigger_name, trigger_group)
      REFERENCES ${TablePrefix}triggers (sched_name, trigger_name, trigger_group)
      ON DELETE CASCADE
);

CREATE TABLE ${TablePrefix}simprop_triggers
  (
    sched_name TEXT NOT NULL,
    trigger_name TEXT NOT NULL,
    trigger_group TEXT NOT NULL,
    str_prop_1 TEXT NULL,
    str_prop_2 TEXT NULL,
    str_prop_3 TEXT NULL,
    int_prop_1 INTEGER NULL,
    int_prop_2 INTEGER NULL,
    long_prop_1 BIGINT NULL,
    long_prop_2 BIGINT NULL,
    dec_prop_1 NUMERIC NULL,
    dec_prop_2 NUMERIC NULL,
    bool_prop_1 BOOL NULL,
    bool_prop_2 BOOL NULL,
    time_zone_id TEXT NULL,
    PRIMARY KEY (sched_name, trigger_name, trigger_group),
    FOREIGN KEY (sched_name, trigger_name, trigger_group)
      REFERENCES ${TablePrefix}triggers (sched_name, trigger_name, trigger_group)
      ON DELETE CASCADE
);

CREATE TABLE ${TablePrefix}cron_triggers
  (
    sched_name TEXT NOT NULL,
    trigger_name TEXT NOT NULL,
    trigger_group TEXT NOT NULL,
    cron_expression TEXT NOT NULL,
    time_zone_id TEXT,
    PRIMARY KEY (sched_name, trigger_name, trigger_group),
    FOREIGN KEY (sched_name, trigger_name, trigger_group)
      REFERENCES ${TablePrefix}triggers (sched_name, trigger_name, trigger_group)
      ON DELETE CASCADE
);

CREATE TABLE ${TablePrefix}blob_triggers
  (
    sched_name TEXT NOT NULL,
    trigger_name TEXT NOT NULL,
    trigger_group TEXT NOT NULL,
    blob_data BYTEA NULL,
    PRIMARY KEY (sched_name, trigger_name, trigger_group),
    FOREIGN KEY (sched_name, trigger_name, trigger_group)
      REFERENCES ${TablePrefix}triggers (sched_name, trigger_name, trigger_group)
      ON DELETE CASCADE
);

CREATE TABLE ${TablePrefix}calendars
  (
    sched_name TEXT NOT NULL,
    calendar_name TEXT NOT NULL,
    calendar BYTEA NOT NULL,
    PRIMARY KEY (sched_name, calendar_name)
);

CREATE TABLE ${TablePrefix}paused_trigger_grps
  (
    sched_name TEXT NOT NULL,
    trigger_group TEXT NOT NULL,
    PRIMARY KEY (sched_name, trigger_group)
);

CREATE TABLE ${TablePrefix}fired_triggers
  (
    sched_name TEXT NOT NULL,
    entry_id TEXT NOT NULL,
    trigger_name TEXT NOT NULL,
    trigger_group TEXT NOT NULL,
    instance_name TEXT NOT NULL,
    fired_time BIGINT NOT NULL,
    sched_time BIGINT NOT NULL,
    priority INTEGER NOT NULL,
    state TEXT NOT NULL,
    job_name TEXT NULL,
    job_group TEXT NULL,
    is_nonconcurrent BOOL NOT NULL,
    requests_recovery BOOL NULL,
    PRIMARY KEY (sched_name, entry_id)
);

CREATE TABLE ${TablePrefix}scheduler_state
  (
    sched_name TEXT NOT NULL,
    instance_name TEXT NOT NULL,
    last_checkin_time BIGINT NOT NULL,
    checkin_interval BIGINT NOT NULL,
    PRIMARY KEY (sched_name, instance_name)
);

CREATE TABLE ${TablePrefix}locks
  (
    sched_name TEXT NOT NULL,
    lock_name TEXT NOT NULL,
    PRIMARY KEY (sched_name, lock_name)
);

CREATE INDEX idx_${TablePrefix}j_req_recovery ON ${TablePrefix}job_details (requests_recovery);
CREATE INDEX idx_${TablePrefix}t_next_fire_time ON ${TablePrefix}triggers (next_fire_time);
CREATE INDEX idx_${TablePrefix}t_state ON ${TablePrefix}triggers (trigger_state);
CREATE INDEX idx_${TablePrefix}t_nft_st ON ${TablePrefix}triggers (next_fire_time, trigger_state);
CREATE INDEX idx_${TablePrefix}ft_trig_name ON ${TablePrefix}fired_triggers (trigger_name);
CREATE INDEX idx_${TablePrefix}ft_trig_group ON ${TablePrefix}fired_triggers (trigger_group);
CREATE INDEX idx_${TablePrefix}ft_trig_nm_gp ON ${TablePrefix}fired_triggers (sched_name, trigger_name, trigger_group);
CREATE INDEX idx_${TablePrefix}ft_trig_inst_name ON ${TablePrefix}fired_triggers (instance_name);
CREATE INDEX idx_${TablePrefix}ft_job_name ON ${TablePrefix}fired_triggers (job_name);
CREATE INDEX idx_${TablePrefix}ft_job_group ON ${TablePrefix}fired_triggers (job_group);
CREATE INDEX idx_${TablePrefix}ft_job_req_recovery ON ${TablePrefix}fired_triggers (requests_recovery);